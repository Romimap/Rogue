using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Bird : MonoBehaviour {
    [System.Serializable]
    public struct Data {
        public int index;

        public float3 position;
        public float3 velocity;

        public float clumpFactor;
        public float evadeFactor;
        public float alignFactor;
        public float targetFactor;
        public float velocityFactor;

        public float speed;
        
        public float visionRadius;
    }

    public Data m_data;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        m_data.position += m_data.velocity * m_data.speed * Time.deltaTime;
        transform.position = m_data.position;
    }
}
