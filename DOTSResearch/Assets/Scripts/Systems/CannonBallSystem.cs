using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
partial struct CannonBallJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ECB;
    public float DeltaTime;

    void Execute([ChunkIndexInQuery] int chunkIndex, ref CannonBallAspect cannonBall)
    {
        var gravity = new float3(0.0f,-9.82f,0.0f);
        var invertY = new float3(1.0f,-1.0f,1.0f);

        cannonBall.Position += cannonBall.Speed * DeltaTime;
        if (cannonBall.Position.y < 0.0f)
        {
            cannonBall.Position *= invertY;
            cannonBall.Speed *= invertY * 0.8f;
        }

        cannonBall.Speed *= gravity * DeltaTime;

        var speed = math.lengthsq(cannonBall.Speed);
        if (speed < 0.1f)
        {
            ECB.DestroyEntity(chunkIndex,cannonBall.Self);
        }
    }
}

[BurstCompile]
partial struct CannonBallSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var single = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = single.CreateCommandBuffer(state.WorldUnmanaged);
        var cannonBallJob = new CannonBallJob()
        {
            ECB = ecb.AsParallelWriter(),
            DeltaTime =  SystemAPI.Time.DeltaTime
        };
        cannonBallJob.ScheduleParallel();
    }

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
}
