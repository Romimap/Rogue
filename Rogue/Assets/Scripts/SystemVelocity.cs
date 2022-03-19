using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class SystemVelocity : SystemBase {
    protected override void OnUpdate() {
        float deltaTime = Time.DeltaTime;
        
        Entities.ForEach((ref Translation translation, in Velocity velocity) => {
            translation.Value += velocity.Value * deltaTime;
        }).Schedule();
    }
}
