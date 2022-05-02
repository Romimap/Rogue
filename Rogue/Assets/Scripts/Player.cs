using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Player : MonoBehaviour
{
    public static Player Singleton;
    public float m_speed;
    public float m_acceleration;
    public Terrain m_terrain;

    public Vector3 velocity;
    // Start is called before the first frame update
    void Start() {
        Singleton = this;
    }

    // Update is called once per frame
    void Update() {
        Vector3 myPos = transform.position;
        myPos += velocity * m_speed * Time.deltaTime;
        myPos.y =  m_terrain.SampleHeight(myPos);
       // transform.position = myPos;
    }

    void FixedUpdate () {
        Vector3 direction = new Vector3();

        direction.x += Input.GetAxis("Horizontal");
        direction.z += Input.GetAxis("Vertical");

        direction = direction.normalized;

        velocity = direction * m_acceleration + velocity * (1 - m_acceleration);
    }
}
