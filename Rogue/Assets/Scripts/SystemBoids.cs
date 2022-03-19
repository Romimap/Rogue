using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class SystemBoids : SystemBase {
    KNN.KnnContainer knnContainer;
    NativeArray<float3> knnPositions;
    NativeArray<Entity> birds;

    const int MAXBIRDS = 1000;

    protected override void OnCreate() {
        knnPositions = new NativeArray<float3>(MAXBIRDS, Allocator.Persistent);
        knnContainer = new KNN.KnnContainer(knnPositions, true, Allocator.Persistent);
        birds = new NativeArray<Entity>(MAXBIRDS, Allocator.Persistent);
    }

    protected override void OnUpdate() {
        ComponentDataFromEntity<Velocity> velocities = GetComponentDataFromEntity<Velocity>(true);
        ComponentDataFromEntity<Translation> translations = GetComponentDataFromEntity<Translation>(true);
        ComponentDataFromEntity<BirdData> birdDatas = GetComponentDataFromEntity<BirdData>(true);
        
        for (int i = 0; i < birds.Length; i++) {
            knnPositions[i] = translations[birds[i]].Value;
        }
        
        var rebuildJob = new KNN.Jobs.KnnRebuildJob(knnContainer);
        rebuildJob.Schedule().Complete();
      
        KNN.KnnContainer lknnContainer = knnContainer;
        NativeArray<Entity>.ReadOnly lBirds = birds.AsReadOnly();
        
        Entities.
        WithReadOnly(velocities).
        WithReadOnly(translations).
        WithReadOnly(birdDatas).
        WithReadOnly(lknnContainer).
        WithReadOnly(lBirds).
        ForEach((ref Velocity velocity, in Translation translation, in BirdData birdData) => {
            if (!birdData.active) return;

            NativeList<int> ids = new NativeList<int>();
            lknnContainer.QueryRange(translation.Value, birdData.range, ids);

            float3 evade = new float3(0);
            float3 clump = new float3(0);
            float3 align = new float3(0);
            float3 target = new float3(0);

            float3 avgposition = new float3(0);
            int nSameGroup = 0;
            int nTotal = 0;
            float wSameGroup = 0;
            float wTotal = 0;

            foreach(int id in ids) {
                Entity closeBird = lBirds[id];

                float distance = math.distance(translation.Value, translations[closeBird].Value);
                float w = 1.0f - distance / birdData.range;

                if (birdDatas[closeBird].group == birdData.group) {
                    avgposition += translations[closeBird].Value;
                    align += velocities[closeBird].Value * w;
                    evade += math.normalize(translation.Value - translations[closeBird].Value) * w;
                } else {
                    evade += math.normalize(translation.Value - translations[closeBird].Value) * w * 2;
                }

                if (birdDatas[closeBird].group == birdData.group) {
                    nSameGroup++;
                    wSameGroup += w;
                } 
                nTotal++;
                wTotal += w;
            }

            {
                avgposition /= nSameGroup;
                float distance = math.distance(translation.Value, avgposition);
                float w = 1.0f - distance / birdData.range;
                clump = math.normalize(avgposition - translation.Value) * w;

                evade /= wTotal;
                align /= wSameGroup;
                target =  math.normalize(-translation.Value);
            }

            velocity.Value = 
              birdData.evadeFactor * evade 
            + birdData.clumpFactor * clump 
            + birdData.alignFactor * align 
            + birdData.targetFactor * target
            + birdData.velocityFactor * velocity.Value;

            velocity.Value /=  
              birdData.evadeFactor 
            + birdData.clumpFactor 
            + birdData.alignFactor 
            + birdData.targetFactor
            + birdData.velocityFactor;
        }).ScheduleParallel();
    }
}
