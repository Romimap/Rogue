using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
public class AuthoringBird : MonoBehaviour, IConvertGameObjectToEntity {
    public float _range;
    public float _speed;
    public int _group;
    public float _evadeFactor;
    public float _clumpFactor;
    public float _alignFactor;
    public float _targetFactor;
    public float _velocityFactor; 
    
    

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.AddComponentData(entity, new BirdData {
            range = _range,
            speed = _speed,
            group = _group,
            evadeFactor = _evadeFactor,
            clumpFactor = _clumpFactor,
            alignFactor = _alignFactor,
            targetFactor = _targetFactor,
            velocityFactor = _velocityFactor
        });

        dstManager.AddComponentData(entity, new Velocity {
            Value = new float3(0)
        }); 
    }
}
