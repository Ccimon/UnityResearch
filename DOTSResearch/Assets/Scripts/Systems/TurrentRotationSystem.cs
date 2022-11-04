using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
partial struct TurrentRotationSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }
    
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        var rotation = quaternion.RotateY(SystemAPI.Time.DeltaTime * math.PI);

        foreach (var transform in SystemAPI.Query<TransformAspect>().WithAll<Turrent>())
        {
            transform.RotateWorld(rotation);
        }
    }
}
