using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct Velocity : IComponentData {
    public float3 Value;
}

[Serializable]
public struct BirdData : IComponentData {
    public bool active;
    public float range;
    public float speed;
    public int group;

    public float evadeFactor;
    public float clumpFactor;
    public float alignFactor;
    public float targetFactor;
    public float velocityFactor;
}

