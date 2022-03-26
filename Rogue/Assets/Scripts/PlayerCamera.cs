using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position * 0.95f + (Player.Singleton.transform.position + new Vector3(0, 10, -10)) * 0.05f;
        transform.LookAt(transform.position - new Vector3(0, 10, -10), Vector3.up);
    }
}
