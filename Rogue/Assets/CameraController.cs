using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float azimuth = 45;
    float elevation = 45;
    public float sensibility = 0.5f;
    Vector2 mousePos = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        mousePos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update() {
        Vector2 delta = (Vector2)Input.mousePosition - mousePos;

        elevation -= delta.y * sensibility;
        azimuth -= delta.x * sensibility;

        if (elevation < 0) elevation = 0;
        if (elevation > 90) elevation = 90;

        Quaternion current = transform.rotation;
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, azimuth);
        transform.Rotate(Vector3.right, elevation);
        Quaternion target = transform.rotation;

        transform.rotation = Quaternion.Lerp(current, target, 0.1f);


        mousePos = Input.mousePosition;
    }
}
