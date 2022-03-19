using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour {
    [SerializeField] private LayerMask terrainLayer = default;
    [SerializeField] private Transform body = default;
    [SerializeField] private IKFootSolver otherFoot = default;
    [SerializeField] private IKFootSolver friendFoot = default;
    [SerializeField] private float speed = 1;
    [SerializeField] private float stepDistance = 4;
    [SerializeField] private float stepHeight = 1;
    [SerializeField] private float stepSpeed = 5;
    [SerializeField] private Vector3 footOffset = default;

    private Vector3 oldPosition, currentPosition, newPosition;
    private Vector3 oldNormal, currentNormal, newNormal;

    public enum StepState {STEPPING, IDLE};

    public StepState stepState = StepState.IDLE;

    private void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Debug.DrawRay(body.TransformPoint(footOffset), Vector3.down * 10, Color.red);
        transform.position = currentPosition;

        if (stepState == StepState.STEPPING) return;

        //Raycast down @ footoffset
        RaycastHit hit;
        if (Physics.Raycast(body.TransformPoint(footOffset), Vector3.down, out hit, Mathf.Infinity, terrainLayer)) {
            if ((hit.point - currentPosition).magnitude > stepDistance) {
                Step();
                Debug.Log("MAG: " + (hit.point - currentPosition).magnitude);
            }
        }
    }

    IEnumerator StepAnimation () {
        stepState = StepState.STEPPING;

        //TODO, Animate step
        Debug.Log("START");
        currentPosition = newPosition;

        float t = 1;
        while (t > 0) {
            currentPosition = t * oldPosition + (1 - t) * newPosition;
            currentPosition.y += Mathf.Sin(t * Mathf.PI) * stepHeight;
            t -= Time.deltaTime * stepSpeed;
            yield return new WaitForEndOfFrame();
        }

        currentPosition = newPosition;

        Debug.Log("END");
        stepState = StepState.IDLE;
    }

    public void Step () {
        if (otherFoot.stepState == StepState.STEPPING) return;
        if (stepState == StepState.STEPPING) return;
        
        RaycastHit hit;
        if (Physics.Raycast(body.TransformPoint(footOffset), Vector3.down, out hit, Mathf.Infinity, terrainLayer)) {

            Vector3 targetPos = hit.point + (stepDistance * 0.9f) * (hit.point - currentPosition).normalized;

            if (Physics.Raycast(targetPos, Vector3.down, out hit, Mathf.Infinity, terrainLayer)) {
                Debug.DrawRay(body.TransformPoint(footOffset), Vector3.down * hit.distance, Color.green);
                oldPosition = currentPosition;
                newPosition = hit.point;

                StartCoroutine("StepAnimation");
                friendFoot.Step();
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(newPosition, 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(currentPosition, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(oldPosition, 0.2f);
    }
}
