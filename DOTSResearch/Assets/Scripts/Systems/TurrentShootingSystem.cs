using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
partial struct TurrentShootingSystem : ISystem
{
    private ComponentLookup<LocalToWorldTransform> m_LocalToWorldTransformFromEntity;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        m_LocalToWorldTransformFromEntity = state.GetComponentLookup<LocalToWorldTransform>(true);
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        m_LocalToWorldTransformFromEntity.Update(ref state);
        var single = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = single.CreateCommandBuffer(state.WorldUnmanaged);

        var job = new TurrentShoot
        {
            LocalToWorldTransformFromEntity = m_LocalToWorldTransformFromEntity,
            ECB = ecb
        };

        job.Schedule();
    }
    
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

[BurstCompile]
partial struct TurrentShoot : IJobEntity
{
    [ReadOnly] public ComponentLookup<LocalToWorldTransform> LocalToWorldTransformFromEntity;
    public EntityCommandBuffer ECB;

    void Execute(in TurrentAspect turrent)
    {
        var instance = ECB.Instantiate(turrent.CannonBallPrefab);
        var spawnLocalToWorld = LocalToWorldTransformFromEntity[turrent.CannonBallSpawn];
        var cannonballTransform = UniformScaleTransform.FromPosition(spawnLocalToWorld.Value.Position);

        cannonballTransform.Scale = LocalToWorldTransformFromEntity[turrent.CannonBallPrefab].Value.Scale;
        ECB.SetComponent(instance,new LocalToWorldTransform()
        {
            Value = cannonballTransform
        });
        ECB.SetComponent(instance,new Cannonball()
        {
            Speed = spawnLocalToWorld.Value.Forward() * 20.0f
        });
    }
}