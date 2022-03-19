#pragma warning disable 0219
#line 1 "/home/romimap/Documents/git/Rogue/Temp/GeneratedCode/Assembly-CSharp/SystemVelocity__System_1040918010.g.cs"
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[System.Runtime.CompilerServices.CompilerGenerated]
public partial class SystemVelocity : SystemBase
{
    [Unity.Entities.DOTSCompilerPatchedMethod("OnUpdate")]
    protected void __OnUpdate_2AA99631()
    {
        #line 10 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemVelocity.cs"
        float deltaTime = Time.DeltaTime;
        #line 12 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemVelocity.cs"
        SystemVelocity_LambdaJob_0_Execute(deltaTime);
    }

    Unity.Entities.EntityQuery SystemVelocity_LambdaJob_0_Query;
    Unity.Entities.ComponentTypeHandle<Unity.Transforms.Translation> __Unity_Transforms_Translation_RW_ComponentTypeHandle;
    Unity.Entities.ComponentTypeHandle<Velocity> __Velocity_RO_ComponentTypeHandle;
    #line 26 "/home/romimap/Documents/git/Rogue/Temp/GeneratedCode/Assembly-CSharp/SystemVelocity__System_1040918010.g.cs"
    [Unity.Burst.NoAlias]
    [Unity.Burst.BurstCompile(FloatMode = Unity.Burst.FloatMode.Default, FloatPrecision = Unity.Burst.FloatPrecision.Standard, CompileSynchronously = false)]
    struct SystemVelocity_LambdaJob_0_Job : Unity.Entities.IJobEntityBatch
    {
        [Unity.Collections.ReadOnly]
        public ulong __worldSequenceNumber;
        [Unity.Collections.ReadOnly]
        public Unity.Entities.SystemHandleUntyped __executingSystem;
        public float deltaTime;
        public Unity.Entities.ComponentTypeHandle<Unity.Transforms.Translation> __translationTypeHandle;
        [Unity.Collections.ReadOnly]
        public Unity.Entities.ComponentTypeHandle<Velocity> __velocityTypeHandle;
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void OriginalLambdaBody([Unity.Burst.NoAlias] ref Unity.Transforms.Translation translation, [Unity.Burst.NoAlias] in Velocity velocity)
        {
            #line 13 "/home/romimap/Documents/git/Rogue/Rogue/Assets/Scripts/SystemVelocity.cs"
            translation.Value += velocity.Value * deltaTime;
        }

        #line 46 "/home/romimap/Documents/git/Rogue/Temp/GeneratedCode/Assembly-CSharp/SystemVelocity__System_1040918010.g.cs"
        public void Execute(Unity.Entities.ArchetypeChunk chunk, int batchIndex)
        {
            var translationArrayPtr = Unity.Entities.InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<Unity.Transforms.Translation>(chunk, __translationTypeHandle);
            var velocityArrayPtr = Unity.Entities.InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<Velocity>(chunk, __velocityTypeHandle);
            int count = chunk.Count;
            for (int entityIndex = 0; entityIndex != count; entityIndex++)
            {
                OriginalLambdaBody(ref Unity.Entities.InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Unity.Transforms.Translation>(translationArrayPtr, entityIndex), in Unity.Entities.InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Velocity>(velocityArrayPtr, entityIndex));
            }

            if (Unity.Entities.EntitiesJournaling.Enabled)
                EntitiesJournaling_RecordChunk(in chunk, in __translationTypeHandle, translationArrayPtr);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        void EntitiesJournaling_RecordChunk(in ArchetypeChunk chunk, in Unity.Entities.ComponentTypeHandle<Unity.Transforms.Translation> translationTypeHandle, System.IntPtr translationArrayPtr)
        {
            Unity.Entities.InternalCompilerInterface.EntitiesJournaling_RecordSetComponentData(__worldSequenceNumber, in __executingSystem, chunk, translationTypeHandle, translationArrayPtr);
        }
    }

    void SystemVelocity_LambdaJob_0_Execute(float deltaTime)
    {
        __Unity_Transforms_Translation_RW_ComponentTypeHandle.Update(this);
        __Velocity_RO_ComponentTypeHandle.Update(this);
        var __job = new SystemVelocity_LambdaJob_0_Job{__worldSequenceNumber = this.World.SequenceNumber, __executingSystem = this.SystemHandleUntyped, deltaTime = deltaTime, __translationTypeHandle = __Unity_Transforms_Translation_RW_ComponentTypeHandle, __velocityTypeHandle = __Velocity_RO_ComponentTypeHandle};
        Dependency = Unity.Entities.JobEntityBatchExtensions.Schedule(__job, SystemVelocity_LambdaJob_0_Query, Dependency);
    }

    protected override void OnCreateForCompiler()
    {
        base.OnCreateForCompiler();
        SystemVelocity_LambdaJob_0_Query = GetEntityQuery(new Unity.Entities.EntityQueryDesc{All = new Unity.Entities.ComponentType[]{ComponentType.ReadOnly<Velocity>(), Unity.Entities.ComponentType.ReadWrite<Unity.Transforms.Translation>()}, Any = new Unity.Entities.ComponentType[]{}, None = new Unity.Entities.ComponentType[]{}, Options = Unity.Entities.EntityQueryOptions.Default});
        __Unity_Transforms_Translation_RW_ComponentTypeHandle = GetComponentTypeHandle<Unity.Transforms.Translation>(false);
        ;
        __Velocity_RO_ComponentTypeHandle = GetComponentTypeHandle<Velocity>(true);
    }
}