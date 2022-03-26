using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Bird : MonoBehaviour {
    [System.Serializable]
    public struct Data {
        public int index;

        public int group;

        public float3 position;
        public float3 velocity;

        public float clumpFactor;
        public float evadeFactor;
        public float alignFactor;
        public float targetFactor;
        public float velocityFactor;

        public float otherGroupEvade;

        public float speed;
        
        public float visionRadius;
    }

    public Data m_data;

    private Quaternion desiredRotation;

    // Start is called before the first frame update
    void Start() {
        desiredRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update() {
        m_data.position += m_data.velocity * m_data.speed * Time.deltaTime;

        m_data.position.y =  Player.Singleton.m_terrain.SampleHeight(m_data.position);

        transform.position = m_data.position;
        Transform t = transform;
        t.LookAt(m_data.position + m_data.velocity, Vector3.up);
        desiredRotation = t.rotation;


        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 0.1f);
    }
}
