#pragma warning disable 0219
#line 1 "/home/romimap/Documents/git/Rogue/Temp/GeneratedCode/Assembly-CSharp/SystemBoids__System_620297396.g.cs"
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[System.Runtime.CompilerServices.CompilerGenerated]
public partial class SystemBoids : SystemBase
{
    [Unity.Entities.DOTSCompilerPatchedMethod("OnUpdate")]
    protected void __OnUpdate_4889DB69()
    {
        #line 22 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
        ComponentDataFromEntity<Velocity> velocities = GetComponentDataFromEntity<Velocity>(true);
        #line 23 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
        ComponentDataFromEntity<Translation> translations = GetComponentDataFromEntity<Translation>(true);
        #line 24 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
        ComponentDataFromEntity<BirdData> birdDatas = GetComponentDataFromEntity<BirdData>(true);
        #line 26 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
        for (int i = 0; i < birds.Length; i++)
        {
            #line 27 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            knnPositions[i] = translations[birds[i]].Value;
        }

        #line 30 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
        var rebuildJob = new KNN.Jobs.KnnRebuildJob(knnContainer);
        #line 31 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
        rebuildJob.Schedule().Complete();
        #line 33 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
        KNN.KnnContainer lknnContainer = knnContainer;
        #line 34 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
        NativeArray<Entity>.ReadOnly lBirds = birds.AsReadOnly();
        #line 36 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
        SystemBoids_LambdaJob_0_Execute(velocities, translations, birdDatas, lknnContainer, lBirds);
    }

    Unity.Entities.EntityQuery SystemBoids_LambdaJob_0_Query;
    Unity.Entities.ComponentTypeHandle<Velocity> __Velocity_RW_ComponentTypeHandle;
    Unity.Entities.ComponentTypeHandle<Unity.Transforms.Translation> __Unity_Transforms_Translation_RO_ComponentTypeHandle;
    Unity.Entities.ComponentTypeHandle<BirdData> __BirdData_RO_ComponentTypeHandle;
    #line 46 "/home/romimap/Documents/git/Rogue/Temp/GeneratedCode/Assembly-CSharp/SystemBoids__System_620297396.g.cs"
    [Unity.Burst.NoAlias]
    [Unity.Burst.BurstCompile(FloatMode = Unity.Burst.FloatMode.Default, FloatPrecision = Unity.Burst.FloatPrecision.Standard, CompileSynchronously = false)]
    struct SystemBoids_LambdaJob_0_Job : Unity.Entities.IJobEntityBatch
    {
        [Unity.Collections.ReadOnly]
        public ulong __worldSequenceNumber;
        [Unity.Collections.ReadOnly]
        public Unity.Entities.SystemHandleUntyped __executingSystem;
        [Unity.Collections.ReadOnly]
        public Unity.Entities.ComponentDataFromEntity<Velocity> velocities;
        [Unity.Collections.ReadOnly]
        public Unity.Entities.ComponentDataFromEntity<Unity.Transforms.Translation> translations;
        [Unity.Collections.ReadOnly]
        public Unity.Entities.ComponentDataFromEntity<BirdData> birdDatas;
        [Unity.Collections.ReadOnly]
        public KNN.KnnContainer lknnContainer;
        [Unity.Collections.ReadOnly]
        public Unity.Collections.NativeArray<Unity.Entities.Entity>.ReadOnly lBirds;
        public Unity.Entities.ComponentTypeHandle<Velocity> __velocityTypeHandle;
        [Unity.Collections.ReadOnly]
        public Unity.Entities.ComponentTypeHandle<Unity.Transforms.Translation> __translationTypeHandle;
        [Unity.Collections.ReadOnly]
        public Unity.Entities.ComponentTypeHandle<BirdData> __birdDataTypeHandle;
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void OriginalLambdaBody([Unity.Burst.NoAlias] ref Velocity velocity, [Unity.Burst.NoAlias] in Unity.Transforms.Translation translation, [Unity.Burst.NoAlias] in BirdData birdData)
        {
            #line 43 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            if (!birdData.active)
                #line 43 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                return;
            #line 45 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            NativeList<int> ids = new NativeList<int>();
            #line 46 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            lknnContainer.QueryRange(translation.Value, birdData.range, ids);
            #line 48 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            float3 evade = new float3(0);
            #line 49 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            float3 clump = new float3(0);
            #line 50 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            float3 align = new float3(0);
            #line 51 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            float3 target = new float3(0);
            #line 53 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            float3 avgposition = new float3(0);
            #line 54 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            int nSameGroup = 0;
            #line 55 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            int nTotal = 0;
            #line 56 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            float wSameGroup = 0;
            #line 57 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            float wTotal = 0;
            #line 59 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            foreach (int id in ids)
            {
                #line 60 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                Entity closeBird = lBirds[id];
                #line 62 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                float distance = math.distance(translation.Value, translations[closeBird].Value);
                #line 63 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                float w = 1.0f - distance / birdData.range;
                #line 65 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                if (birdDatas[closeBird].group == birdData.group)
                {
                    #line 66 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                    avgposition += translations[closeBird].Value;
                    #line 67 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                    align += velocities[closeBird].Value * w;
                    #line 68 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                    evade += math.normalize(translation.Value - translations[closeBird].Value) * w;
                }
                else
                {
                    #line 70 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                    evade += math.normalize(translation.Value - translations[closeBird].Value) * w * 2;
                }

                #line 73 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                if (birdDatas[closeBird].group == birdData.group)
                {
                    #line 74 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                    nSameGroup++;
                    #line 75 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                    wSameGroup += w;
                }

                #line 77 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                nTotal++;
                #line 78 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                wTotal += w;
            }

            {
                #line 82 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                avgposition /= nSameGroup;
                #line 83 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                float distance = math.distance(translation.Value, avgposition);
                #line 84 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                float w = 1.0f - distance / birdData.range;
                #line 85 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                clump = math.normalize(avgposition - translation.Value) * w;
                #line 87 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                evade /= wTotal;
                #line 88 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                align /= wSameGroup;
                #line 89 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
                target = math.normalize(-translation.Value);
            }

            #line 92 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            velocity.Value = birdData.evadeFactor * evade + birdData.clumpFactor * clump + birdData.alignFactor * align + birdData.targetFactor * target + birdData.velocityFactor * velocity.Value;
            #line 99 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemBoids.cs"
            velocity.Value /= birdData.evadeFactor + birdData.clumpFactor + birdData.alignFactor + birdData.targetFactor + birdData.velocityFactor;
        }

        #line 162 "/home/romimap/Documents/git/Rogue/Temp/GeneratedCode/Assembly-CSharp/SystemBoids__System_620297396.g.cs"
        public void Execute(Unity.Entities.ArchetypeChunk chunk, int batchIndex)
        {
            var velocityArrayPtr = Unity.Entities.InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<Velocity>(chunk, __velocityTypeHandle);
            var translationArrayPtr = Unity.Entities.InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<Unity.Transforms.Translation>(chunk, __translationTypeHandle);
            var birdDataArrayPtr = Unity.Entities.InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<BirdData>(chunk, __birdDataTypeHandle);
            int count = chunk.Count;
            for (int entityIndex = 0; entityIndex != count; entityIndex++)
            {
                OriginalLambdaBody(ref Unity.Entities.InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Velocity>(velocityArrayPtr, entityIndex), in Unity.Entities.InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Unity.Transforms.Translation>(translationArrayPtr, entityIndex), in Unity.Entities.InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<BirdData>(birdDataArrayPtr, entityIndex));
            }

            if (Unity.Entities.EntitiesJournaling.Enabled)
                EntitiesJournaling_RecordChunk(in chunk, in __velocityTypeHandle, velocityArrayPtr);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        void EntitiesJournaling_RecordChunk(in ArchetypeChunk chunk, in Unity.Entities.ComponentTypeHandle<Velocity> velocityTypeHandle, System.IntPtr velocityArrayPtr)
        {
            Unity.Entities.InternalCompilerInterface.EntitiesJournaling_RecordSetComponentData(__worldSequenceNumber, in __executingSystem, chunk, velocityTypeHandle, velocityArrayPtr);
        }
    }

    void SystemBoids_LambdaJob_0_Execute(Unity.Entities.ComponentDataFromEntity<Velocity> velocities, Unity.Entities.ComponentDataFromEntity<Unity.Transforms.Translation> translations, Unity.Entities.ComponentDataFromEntity<BirdData> birdDatas, KNN.KnnContainer lknnContainer, Unity.Collections.NativeArray<Unity.Entities.Entity>.ReadOnly lBirds)
    {
        __Velocity_RW_ComponentTypeHandle.Update(this);
        __Unity_Transforms_Translation_RO_ComponentTypeHandle.Update(this);
        __BirdData_RO_ComponentTypeHandle.Update(this);
        var __job = new SystemBoids_LambdaJob_0_Job{__worldSequenceNumber = this.World.SequenceNumber, __executingSystem = this.SystemHandleUntyped, velocities = velocities, translations = translations, birdDatas = birdDatas, lknnContainer = lknnContainer, lBirds = lBirds, __velocityTypeHandle = __Velocity_RW_ComponentTypeHandle, __translationTypeHandle = __Unity_Transforms_Translation_RO_ComponentTypeHandle, __birdDataTypeHandle = __BirdData_RO_ComponentTypeHandle};
        Dependency = Unity.Entities.JobEntityBatchExtensions.ScheduleParallel(__job, SystemBoids_LambdaJob_0_Query, Dependency);
    }

    protected override void OnCreateForCompiler()
    {
        base.OnCreateForCompiler();
        SystemBoids_LambdaJob_0_Query = GetEntityQuery(new Unity.Entities.EntityQueryDesc{All = new Unity.Entities.ComponentType[]{ComponentType.ReadOnly<Unity.Transforms.Translation>(), ComponentType.ReadOnly<BirdData>(), Unity.Entities.ComponentType.ReadWrite<Velocity>()}, Any = new Unity.Entities.ComponentType[]{}, None = new Unity.Entities.ComponentType[]{}, Options = Unity.Entities.EntityQueryOptions.Default});
        __Velocity_RW_ComponentTypeHandle = GetComponentTypeHandle<Velocity>(false);
        ;
        __Unity_Transforms_Translation_RO_ComponentTypeHandle = GetComponentTypeHandle<Unity.Transforms.Translation>(true);
        ;
        __BirdData_RO_ComponentTypeHandle = GetComponentTypeHandle<BirdData>(true);
    }
}