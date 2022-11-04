#pragma warning disable 0219
#line 1 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Temp/GeneratedCode/Assembly-CSharp/TurrentShootingSystem__System_1942283909.g.cs"
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[global::System.Runtime.CompilerServices.CompilerGenerated]
partial struct TurrentShootingSystem : Unity.Entities.ISystem, Unity.Entities.ISystemCompilerGenerated
{
    [Unity.Entities.DOTSCompilerPatchedMethod("OnUpdate_ref_Unity.Entities.SystemState")]
    void __OnUpdate_6E994214(ref SystemState state)
    {
        #line 23 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TurrentShootingSystem.cs"
        m_LocalToWorldTransformFromEntity.Update(ref state);
        #line 24 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TurrentShootingSystem.cs"
        var single = __query_757551429_1.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        #line 25 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TurrentShootingSystem.cs"
        var ecb = single.CreateCommandBuffer(state.WorldUnmanaged);
        #line 27 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TurrentShootingSystem.cs"
        var job = new TurrentShoot{LocalToWorldTransformFromEntity = m_LocalToWorldTransformFromEntity, ECB = ecb};
        #line 33 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TurrentShootingSystem.cs"
        state.Dependency = __ScheduleViaJobChunkExtension_0(job, __query_757551429_0, state.Dependency, ref state);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    Unity.Jobs.JobHandle __ScheduleViaJobChunkExtension_0(TurrentShoot job, Unity.Entities.EntityQuery entityQuery, Unity.Jobs.JobHandle dependency, ref Unity.Entities.SystemState state)
    {
        __TurrentAspect_RO_AspectTypeHandle.Update(ref state);
        job.__TurrentAspectTypeHandle = __TurrentAspect_RO_AspectTypeHandle;
        return Unity.Entities.JobChunkExtensions.Schedule(job, entityQuery, dependency);
        ;
    }

    Unity.Entities.EntityQuery __query_757551429_0;
    Unity.Entities.EntityQuery __query_757551429_1;
    TurrentAspect.TypeHandle __TurrentAspect_RO_AspectTypeHandle;
    public void OnCreateForCompiler(ref SystemState state)
    {
        __query_757551429_0 = state.GetEntityQuery(new Unity.Entities.EntityQueryDesc{All = TurrentAspect.RequiredComponentsRO, Any = new Unity.Entities.ComponentType[]{}, None = new Unity.Entities.ComponentType[]{}, Options = Unity.Entities.EntityQueryOptions.Default});
        __query_757551429_1 = state.GetEntityQuery(new Unity.Entities.EntityQueryDesc{All = new Unity.Entities.ComponentType[]{Unity.Entities.ComponentType.ReadOnly<Unity.Entities.BeginSimulationEntityCommandBufferSystem.Singleton>()}, Any = new Unity.Entities.ComponentType[]{}, None = new Unity.Entities.ComponentType[]{}, Options = Unity.Entities.EntityQueryOptions.Default | Unity.Entities.EntityQueryOptions.IncludeSystems});
        __TurrentAspect_RO_AspectTypeHandle = new TurrentAspect.TypeHandle(ref state, true);
    }
}