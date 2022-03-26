﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class IKFootSolver : MonoBehaviour {
    [SerializeField] private LayerMask terrainLayer = default;
    [SerializeField] private Transform body = default;
    private float stepForce = 0.1f;
    private float stepLenght = 2;
    private float stepHeight = 1;
    private float stepSpeed = 5;
    [SerializeField] private Vector3 footOffset = default;

    private Vector3 oldPosition, currentPosition, newPosition;
    private Vector3 oldNormal, currentNormal, newNormal;

    private Vector3 prevTargetPos;
    private Vector3 deltaTargetPos;

    public enum StepState {STEPPING, IDLE};

    public StepState stepState = StepState.IDLE;


    public void Init(float stepSpeed, float stepForce, float stepLenght, float stepHeight) {
        this.stepSpeed = stepSpeed + 0.1f;
        this.stepForce = stepForce;
        this.stepHeight = stepHeight;
        this.stepLenght = stepLenght;
    }

    void Start() {
        footOffset.y += 5;
    }

    // Update is called once per frame
    void Update() {
        Debug.DrawRay(body.TransformPoint(footOffset), Vector3.down * 15, Color.red);
        transform.position = currentPosition;

        deltaTargetPos = body.TransformPoint(footOffset) - prevTargetPos;
        prevTargetPos = body.TransformPoint(footOffset);
    }

    IEnumerator StepAnimation () {
        stepState = StepState.STEPPING;

        //TODO, Animate step
        Debug.Log("START");
        currentPosition = newPosition;

        float t = 1;
        float mult = (newPosition - oldPosition).magnitude / 10;
        while (t > 0) {
            currentPosition = t * oldPosition + (1 - t) * newPosition;
            currentPosition.y += Mathf.Sin(t * Mathf.PI) * stepHeight * mult;
            t -= Time.deltaTime * stepSpeed;
            yield return new WaitForEndOfFrame();
        }

        currentPosition = newPosition;

        Debug.Log("END");
        stepState = StepState.IDLE;
    }

    public void Step () {
        oldPosition = currentPosition;
        Vector3 delta = deltaTargetPos * stepForce;
        if (math.lengthsq(delta) > 1) delta = math.normalize(delta);
        newPosition = body.TransformPoint(footOffset) + (delta * stepLenght);
        newPosition.y = Player.Singleton.m_terrain.SampleHeight(body.TransformPoint(footOffset));
        StartCoroutine("StepAnimation");
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
