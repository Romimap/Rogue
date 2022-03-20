using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;

public struct BoidEvaluationJob : IJobParallelFor {
    public NativeArray<Bird.Data> birdDatas;
    [ReadOnly] public NativeArray<Bird.Data> birdDatasReadOnly;
    [ReadOnly] public KNN.KnnContainer knn;
    public Unity.Mathematics.Random r;

    public void Execute (int index) {
        Bird.Data data = birdDatas[index];
        NativeArray<int> result = new NativeArray<int>(10, Allocator.Temp);
        knn.QueryKNearest(data.position, result);

        float3 evade = new float3();
        float3 clump = new float3();
        float3 align = new float3();
        float3 target = new float3();

        float3 avgpos = new float3();
        float n = 0;
        foreach (int resultIndex in result) {
            Bird.Data other = birdDatasReadOnly[resultIndex];
            float distance = math.distance(other.position, data.position);

            if (distance < data.visionRadius && resultIndex != data.index) { //Ignore self
                float w = 1.0f - (distance / data.visionRadius);
                avgpos += other.position;
                evade += (data.position - other.position) * w;
                align += other.velocity * w;
                n += 1;
            }
        }
        if (n > 0) {
            avgpos /= n;
            clump = avgpos - data.position;
            evade /= n;
            align /= n;
        }
        target = math.normalizesafe(-data.position);

        data.velocity = (
              math.normalizesafe(data.velocity) * data.velocityFactor //That line is important, this will allow some elasticity on the speed but still tend to a normalized one
            + evade * data.evadeFactor
            + clump * data.clumpFactor
            + align * data.alignFactor
            + target * data.targetFactor) / 
            (data.velocityFactor + data.evadeFactor + data.clumpFactor + data.alignFactor + data.targetFactor);

        result.Dispose();
        birdDatas[index] = data;
    }
}

public class BoidController : MonoBehaviour {
    const int MAXBIRDS = 500;

    public static BoidController Singleton;

    public GameObject m_birdPrefab;

    //Stores the positions of m_birds
    NativeArray<float3> m_positions;
    GameObject[] m_birds = new GameObject[MAXBIRDS];

    KNN.KnnContainer m_knn;

    Stack<int> m_availableIndices = new Stack<int>(MAXBIRDS);
    List<int> m_usedIndices = new List<int>(MAXBIRDS);

    Unity.Mathematics.Random r;
    // Start is called before the first frame update
    void Start() {
        m_positions = new NativeArray<float3>(MAXBIRDS, Allocator.Persistent);
        m_knn = new KNN.KnnContainer(m_positions, true, Allocator.Persistent);


        r = new Unity.Mathematics.Random(1);
        Singleton = this;

        for (int i = 0; i < MAXBIRDS; i++){
            m_birds[i] = null;
            m_positions[i] = new float3(0, 1000, 0); //NOTE: Maybe clumped points are not optimal for kdtree building
            m_availableIndices.Push(MAXBIRDS - i - 1);
        } 

        for (int i = 0; i < MAXBIRDS; i++) {
            AddBird(m_birdPrefab, r.NextFloat3(-20, 20));
        }
        
    }

    public void AddBird (GameObject prefab, Vector3 position, bool rebuild=true) {
        if (m_availableIndices.Count == 0) return;

        Bird birdComponent = prefab.GetComponent<Bird>();
        if (birdComponent == null) return;

        GameObject bird = Instantiate(prefab, transform);
        bird.transform.position = position;

        int index = m_availableIndices.Pop();
        bird.GetComponent<Bird>().m_data.index = index;
        bird.GetComponent<Bird>().m_data.position = position;
        m_birds[index] = bird;
        m_positions[index] = position;
        m_usedIndices.Add(index);

        if (rebuild) RebuildKDTree();
    }

    public void RemoveBird (int index, bool rebuild=true) {
        if (!m_usedIndices.Contains(index)) return;

        m_availableIndices.Push(index);
        Destroy(m_birds[index].gameObject);
        m_birds[index] = null;
        m_positions[index] = new float3(0, 1000, 0);
        m_usedIndices.Remove(index);

        if (rebuild) RebuildKDTree();
    }

    private void RebuildKDTree () {
        var rebuildJob = new KNN.Jobs.KnnRebuildJob(m_knn);
        rebuildJob.Schedule().Complete();
    }

    // Update is called once per frame
    void Update() {
        BoidEvaluationJob boidEvaluationJob;        
        boidEvaluationJob.birdDatas = new NativeArray<Bird.Data>(m_usedIndices.Count, Allocator.TempJob);
        boidEvaluationJob.birdDatasReadOnly = new NativeArray<Bird.Data>(m_usedIndices.Count, Allocator.TempJob);
        boidEvaluationJob.knn = m_knn;
        boidEvaluationJob.r = r;

        for (int i = 0; i < m_usedIndices.Count; i++) {
            boidEvaluationJob.birdDatas[i] = m_birds[m_usedIndices[i]].GetComponent<Bird>().m_data;
            boidEvaluationJob.birdDatasReadOnly[i] = m_birds[m_usedIndices[i]].GetComponent<Bird>().m_data;
        }

        boidEvaluationJob.Schedule(boidEvaluationJob.birdDatas.Length, 1).Complete();
        //for (int i = 0; i < 1000; i++) {
        //    boidEvaluationJob.Execute(i);
        //}

        for (int i = 0; i < m_usedIndices.Count; i++) {
            m_birds[m_usedIndices[i]].GetComponent<Bird>().m_data = boidEvaluationJob.birdDatas[i];
            m_positions[m_usedIndices[i]] = boidEvaluationJob.birdDatas[i].position;
        }

        RebuildKDTree();
        boidEvaluationJob.birdDatas.Dispose();
        boidEvaluationJob.birdDatasReadOnly.Dispose();
    }
}
