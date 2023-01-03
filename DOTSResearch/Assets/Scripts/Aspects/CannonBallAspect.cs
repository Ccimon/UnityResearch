using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

readonly partial struct CannonBallAspect : IAspect
{
   public readonly Entity Self;
   readonly TransformAspect Transform;
   private readonly RefRW<Cannonball> CannonBall;

   public float3 Position
   {
      get => Transform.Position;
      set => Transform.Position = value;
   }
   
   public float3 Speed
   {
      get => CannonBall.ValueRO.Speed;
      set => CannonBall.ValueRW.Speed = value;
   }
}
