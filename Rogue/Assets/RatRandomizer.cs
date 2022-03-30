using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatRandomizer : MonoBehaviour
{
    public float range = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        float r = Random.Range(-range, range) + 1;
        transform.localScale = transform.localScale * r;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
