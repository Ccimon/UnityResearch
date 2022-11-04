#pragma warning disable 0219
#line 1 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Temp/GeneratedCode/Assembly-CSharp/CannonBallSystem__JobEntity_1015814624.g.cs"
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[global::System.Runtime.CompilerServices.CompilerGenerated]
partial struct CannonBallJob : IJobChunk
{
    public CannonBallAspect.TypeHandle __CannonBallAspectTypeHandle;
    [global::System.Runtime.CompilerServices.CompilerGenerated]
    public void Execute(in ArchetypeChunk batch, int batchIndex, bool useEnabledMask, in Unity.Burst.Intrinsics.v128 chunkEnabledMask)
    {
        var __CannonBallAspectTypeHandleArray = __CannonBallAspectTypeHandle.Resolve(batch);
        int chunkEntityCount = batch.ChunkEntityCount;
        int matchingEntityCount = 0;
        if (!useEnabledMask)
        {
            for (int i = 0; i < chunkEntityCount; ++i)
            {
                var __CannonBallAspectTypeHandleArrayArray = __CannonBallAspectTypeHandleArray[i];
                Execute(batchIndex, ref __CannonBallAspectTypeHandleArrayArray);
                matchingEntityCount++;
            }
        }
        else
        {
            int edgeCount = Unity.Mathematics.math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + Unity.Mathematics.math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1;
            bool useRanges = edgeCount <= 4;
            if (useRanges)
            {
                var enabledMask = chunkEnabledMask;
                int i = 0;
                int batchEndIndex = 0;
                while (EnabledBitUtility.GetNextRange(ref enabledMask, ref i, ref batchEndIndex))
                {
                    while (i < batchEndIndex)
                    {
                        var __CannonBallAspectTypeHandleArrayArray = __CannonBallAspectTypeHandleArray[i];
                        Execute(batchIndex, ref __CannonBallAspectTypeHandleArrayArray);
                        i++;
                        matchingEntityCount++;
                    }
                }
            }
            else
            {
                ulong mask64 = chunkEnabledMask.ULong0;
                int count = Unity.Mathematics.math.min(64, chunkEntityCount);
                for (int i = 0; i < count; ++i)
                {
                    if ((mask64 & 1) != 0)
                    {
                        var __CannonBallAspectTypeHandleArrayArray = __CannonBallAspectTypeHandleArray[i];
                        Execute(batchIndex, ref __CannonBallAspectTypeHandleArrayArray);
                        matchingEntityCount++;
                    }

                    mask64 >>= 1;
                }

                mask64 = chunkEnabledMask.ULong1;
                for (int i = 64; i < chunkEntityCount; ++i)
                {
                    if ((mask64 & 1) != 0)
                    {
                        var __CannonBallAspectTypeHandleArrayArray = __CannonBallAspectTypeHandleArray[i];
                        Execute(batchIndex, ref __CannonBallAspectTypeHandleArrayArray);
                        matchingEntityCount++;
                    }

                    mask64 >>= 1;
                }
            }
        }
    }

    // Emitted to disambiguate scheduling method invocations
    public global::Unity.Jobs.JobHandle Schedule(global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
    public global::Unity.Jobs.JobHandle ScheduleByRef(global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
    public global::Unity.Jobs.JobHandle Schedule(global::Unity.Entities.EntityQuery query, global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
    public global::Unity.Jobs.JobHandle ScheduleByRef(global::Unity.Entities.EntityQuery query, global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
    public void Schedule() => __ThrowCodeGenException();
    public void ScheduleByRef() => __ThrowCodeGenException();
    public void Schedule(global::Unity.Entities.EntityQuery query) => __ThrowCodeGenException();
    public void ScheduleByRef(global::Unity.Entities.EntityQuery query) => __ThrowCodeGenException();
    public global::Unity.Jobs.JobHandle ScheduleParallel(global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
    public global::Unity.Jobs.JobHandle ScheduleParallelByRef(global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
    public global::Unity.Jobs.JobHandle ScheduleParallel(global::Unity.Entities.EntityQuery query, global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
    public global::Unity.Jobs.JobHandle ScheduleParallelByRef(global::Unity.Entities.EntityQuery query, global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
    public global::Unity.Jobs.JobHandle ScheduleParallel(global::Unity.Entities.EntityQuery query, global::Unity.Jobs.JobHandle dependsOn, global::Unity.Collections.NativeArray<int> chunkBaseEntityIndices) => __ThrowCodeGenException();
    public global::Unity.Jobs.JobHandle ScheduleParallelByRef(global::Unity.Entities.EntityQuery query, global::Unity.Jobs.JobHandle dependsOn, global::Unity.Collections.NativeArray<int> chunkBaseEntityIndices) => __ThrowCodeGenException();
    public void ScheduleParallel() => __ThrowCodeGenException();
    public void ScheduleParallelByRef() => __ThrowCodeGenException();
    public void ScheduleParallel(global::Unity.Entities.EntityQuery query) => __ThrowCodeGenException();
    public void ScheduleParallelByRef(global::Unity.Entities.EntityQuery query) => __ThrowCodeGenException();
    public void Run() => __ThrowCodeGenException();
    public void RunByRef() => __ThrowCodeGenException();
    public void Run(global::Unity.Entities.EntityQuery query) => __ThrowCodeGenException();
    public void RunByRef(global::Unity.Entities.EntityQuery query) => __ThrowCodeGenException();
    Unity.Jobs.JobHandle __ThrowCodeGenException() => throw new global::System.Exception("This method should have been replaced by source gen.");
}