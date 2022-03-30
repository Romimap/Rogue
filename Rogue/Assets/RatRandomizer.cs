using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatRandomizer : MonoBehaviour
{
    public float range = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        float r = Random.Range(-range, range);
        transform.localScale += new Vector3(r, r, r);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
