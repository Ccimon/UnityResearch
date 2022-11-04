#pragma warning disable 0219
#line 1 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Temp/GeneratedCode/Assembly-CSharp/TankMoveSystem__System_1549463693.g.cs"
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[global::System.Runtime.CompilerServices.CompilerGenerated]
partial class TankMoveSystem : Unity.Entities.SystemBase
{
    [Unity.Entities.DOTSCompilerPatchedMethod("OnUpdate")]
    void __OnUpdate_1817F1CB()
    {
        #line 13 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TankMoveSystem.cs"
        var dt = this.CheckedStateRef.WorldUnmanaged.Time.DeltaTime;
        #line 15 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TankMoveSystem.cs"
        TankMoveSystem_424B14F_LambdaJob_0_Execute(dt);
    }

    #line 24 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Temp/GeneratedCode/Assembly-CSharp/TankMoveSystem__System_1549463693.g.cs"
    [Unity.Burst.NoAlias]
    [Unity.Burst.BurstCompile(FloatMode = Unity.Burst.FloatMode.Default, FloatPrecision = Unity.Burst.FloatPrecision.Standard, CompileSynchronously = false)]
    struct TankMoveSystem_424B14F_LambdaJob_0_Job : Unity.Entities.IJobChunk
    {
        public float dt;
        public Unity.Transforms.TransformAspect.TypeHandle __transTypeHandle;
        public static readonly Unity.Profiling.ProfilerMarker s_ProfilerMarker = new Unity.Profiling.ProfilerMarker("TankMoveSystem_424B14F_LambdaJob_0.ScheduleParallel");
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void OriginalLambdaBody(Unity.Transforms.TransformAspect trans)
        {
            #line 19 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TankMoveSystem.cs"
            var pos = trans.Position;
            #line 20 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TankMoveSystem.cs"
            var angle = (0.5f + noise.cnoise(pos / 10f)) * 4.0f * (float)Math.PI;
            #line 22 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TankMoveSystem.cs"
            var dir = float3.zero;
            #line 23 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TankMoveSystem.cs"
            math.sincos(angle, out dir.x, out dir.z);
            #line 24 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TankMoveSystem.cs"
            trans.Position += dir * dt * 5.0f;
            #line 25 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TankMoveSystem.cs"
            trans.Rotation = quaternion.RotateY(angle);
        }

        #line 49 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Temp/GeneratedCode/Assembly-CSharp/TankMoveSystem__System_1549463693.g.cs"
        [global::System.Runtime.CompilerServices.CompilerGenerated]
        public void Execute(in ArchetypeChunk chunk, int batchIndex, bool useEnabledMask, in Unity.Burst.Intrinsics.v128 chunkEnabledMask)
        {
            var transArrayPtr = __transTypeHandle.Resolve(chunk);
            int chunkEntityCount = chunk.ChunkEntityCount;
            if (!useEnabledMask)
            {
                for (var entityIndex = 0; entityIndex < chunkEntityCount; ++entityIndex)
                {
                    OriginalLambdaBody(transArrayPtr[entityIndex]);
                }
            }
            else
            {
                int edgeCount = Unity.Mathematics.math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + Unity.Mathematics.math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1;
                bool useRanges = edgeCount <= 4;
                if (useRanges)
                {
                    var enabledMask = chunkEnabledMask;
                    int entityIndex = 0;
                    int batchEndIndex = 0;
                    while (EnabledBitUtility.GetNextRange(ref enabledMask, ref entityIndex, ref batchEndIndex))
                    {
                        while (entityIndex < batchEndIndex)
                        {
                            OriginalLambdaBody(transArrayPtr[entityIndex]);
                            entityIndex++;
                        }
                    }
                }
                else
                {
                    ulong mask64 = chunkEnabledMask.ULong0;
                    int count = Unity.Mathematics.math.min(64, chunkEntityCount);
                    for (var entityIndex = 0; entityIndex < count; ++entityIndex)
                    {
                        if ((mask64 & 1) != 0)
                        {
                            OriginalLambdaBody(transArrayPtr[entityIndex]);
                        }

                        mask64 >>= 1;
                    }

                    mask64 = chunkEnabledMask.ULong1;
                    for (var entityIndex = 64; entityIndex < chunkEntityCount; ++entityIndex)
                    {
                        if ((mask64 & 1) != 0)
                        {
                            OriginalLambdaBody(transArrayPtr[entityIndex]);
                        }

                        mask64 >>= 1;
                    }
                }
            }
        }
    }

    void TankMoveSystem_424B14F_LambdaJob_0_Execute(float dt)
    {
        __Unity_Transforms_TransformAspect_RW_AspectTypeHandle.Update(ref this.CheckedStateRef);
        var __job = new TankMoveSystem_424B14F_LambdaJob_0_Job{dt = dt, __transTypeHandle = __Unity_Transforms_TransformAspect_RW_AspectTypeHandle};
        using (TankMoveSystem_424B14F_LambdaJob_0_Job.s_ProfilerMarker.Auto())
        {
            Dependency = Unity.Entities.JobChunkExtensions.ScheduleParallel(__job, __query_1436443408_0, Dependency);
        }
    }

    Unity.Entities.EntityQuery __query_1436443408_0;
    Unity.Transforms.TransformAspect.TypeHandle __Unity_Transforms_TransformAspect_RW_AspectTypeHandle;
    protected override void OnCreateForCompiler()
    {
        base.OnCreateForCompiler();
        __query_1436443408_0 = this.CheckedStateRef.GetEntityQuery(new Unity.Entities.EntityQueryDesc{All = ComponentType.Combine(new Unity.Entities.ComponentType[]{Unity.Entities.ComponentType.ReadOnly<Tank>()}, Unity.Transforms.TransformAspect.RequiredComponents), Any = new Unity.Entities.ComponentType[]{}, None = new Unity.Entities.ComponentType[]{}, Options = Unity.Entities.EntityQueryOptions.Default});
        __Unity_Transforms_TransformAspect_RW_AspectTypeHandle = new Unity.Transforms.TransformAspect.TypeHandle(ref this.CheckedStateRef, false);
    }
}