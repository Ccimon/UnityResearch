using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial class TankMoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var dt = SystemAPI.Time.DeltaTime;
        
        Entities.WithAll<Tank>()
            .ForEach((TransformAspect trans) =>
            {

                var pos = trans.Position;
                var angle = (0.5f + noise.cnoise(pos / 10f)) * 4.0f * (float)Math.PI;

                var dir = float3.zero;
                math.sincos(angle,out dir.x,out dir.z);
                trans.Position += dir * dt * 5.0f;
                trans.Rotation = quaternion.RotateY(angle);
            }).ScheduleParallel();
    }
}
