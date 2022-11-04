#pragma warning disable 0219
#line 1 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Temp/GeneratedCode/Assembly-CSharp/CannonBallSystem__System_1015814624.g.cs"
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[global::System.Runtime.CompilerServices.CompilerGenerated]
partial struct CannonBallSystem : Unity.Entities.ISystem, Unity.Entities.ISystemCompilerGenerated
{
    [Unity.Entities.DOTSCompilerPatchedMethod("OnUpdate_ref_Unity.Entities.SystemState")]
    void __OnUpdate_6E994214(ref SystemState state)
    {
        #line 42 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/CannonBallSystem.cs"
        var single = __query_1405800833_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        #line 43 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/CannonBallSystem.cs"
        var ecb = single.CreateCommandBuffer(state.WorldUnmanaged);
        #line 44 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/CannonBallSystem.cs"
        var cannonBallJob = new CannonBallJob()
        {ECB = ecb.AsParallelWriter(), DeltaTime = state.WorldUnmanaged.Time.DeltaTime};
        #line 49 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/CannonBallSystem.cs"
        state.Dependency = __ScheduleViaJobChunkExtension_0(cannonBallJob, __query_1405800833_0, state.Dependency, ref state);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    Unity.Jobs.JobHandle __ScheduleViaJobChunkExtension_0(CannonBallJob job, Unity.Entities.EntityQuery entityQuery, Unity.Jobs.JobHandle dependency, ref Unity.Entities.SystemState state)
    {
        __CannonBallAspect_RW_AspectTypeHandle.Update(ref state);
        job.__CannonBallAspectTypeHandle = __CannonBallAspect_RW_AspectTypeHandle;
        return Unity.Entities.JobChunkExtensions.ScheduleParallel(job, entityQuery, dependency);
        ;
    }

    Unity.Entities.EntityQuery __query_1405800833_0;
    Unity.Entities.EntityQuery __query_1405800833_1;
    CannonBallAspect.TypeHandle __CannonBallAspect_RW_AspectTypeHandle;
    public void OnCreateForCompiler(ref SystemState state)
    {
        __query_1405800833_0 = state.GetEntityQuery(new Unity.Entities.EntityQueryDesc{All = CannonBallAspect.RequiredComponents, Any = new Unity.Entities.ComponentType[]{}, None = new Unity.Entities.ComponentType[]{}, Options = Unity.Entities.EntityQueryOptions.Default});
        __query_1405800833_1 = state.GetEntityQuery(new Unity.Entities.EntityQueryDesc{All = new Unity.Entities.ComponentType[]{Unity.Entities.ComponentType.ReadOnly<Unity.Entities.EndSimulationEntityCommandBufferSystem.Singleton>()}, Any = new Unity.Entities.ComponentType[]{}, None = new Unity.Entities.ComponentType[]{}, Options = Unity.Entities.EntityQueryOptions.Default | Unity.Entities.EntityQueryOptions.IncludeSystems});
        __CannonBallAspect_RW_AspectTypeHandle = new CannonBallAspect.TypeHandle(ref state, false);
    }
}