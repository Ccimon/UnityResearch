#pragma warning disable 0219
#line 1 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Temp/GeneratedCode/Assembly-CSharp/TurrentRotationSystem__System_701536112.g.cs"
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[global::System.Runtime.CompilerServices.CompilerGenerated]
partial struct TurrentRotationSystem : Unity.Entities.ISystem, Unity.Entities.ISystemCompilerGenerated
{
    [Unity.Entities.DOTSCompilerPatchedMethod("OnUpdate_ref_Unity.Entities.SystemState")]
    void __OnUpdate_6E994214(ref SystemState state)
    {
        #line 28 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TurrentRotationSystem.cs"
        var rotation = quaternion.RotateY(state.WorldUnmanaged.Time.DeltaTime * math.PI);
            #line 30 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TurrentRotationSystem.cs"
            __Unity_Transforms_TransformAspect_RW_AspectTypeHandle.Update(ref state);
            #line hidden
            __Unity_Transforms_TransformAspect_RW_AspectLookup.Update(ref state);
            #line hidden
            Unity.Transforms.TransformAspect.CompleteDependencyBeforeRW(ref state);
            #line hidden
            foreach (var transform in Unity.Transforms.TransformAspect.Query(__query_59507464_0, __Unity_Transforms_TransformAspect_RW_AspectTypeHandle))
            {
                #line 32 "/Users/ccimonyang/Desktop/GitHub/UnityResearch/DOTSResearch/Assets/Scripts/Systems/TurrentRotationSystem.cs"
                transform.RotateWorld(rotation);
            }
    }

    Unity.Entities.EntityQuery __query_59507464_0;
    Unity.Transforms.TransformAspect.TypeHandle __Unity_Transforms_TransformAspect_RW_AspectTypeHandle;
    Unity.Transforms.TransformAspect.Lookup __Unity_Transforms_TransformAspect_RW_AspectLookup;
    public void OnCreateForCompiler(ref SystemState state)
    {
        __query_59507464_0 = state.GetEntityQuery(new Unity.Entities.EntityQueryDesc{All = ComponentType.Combine(new Unity.Entities.ComponentType[]{Unity.Entities.ComponentType.ReadOnly<Turrent>()}, Unity.Transforms.TransformAspect.RequiredComponents), Any = new Unity.Entities.ComponentType[]{}, None = new Unity.Entities.ComponentType[]{}, Options = Unity.Entities.EntityQueryOptions.Default});
        __Unity_Transforms_TransformAspect_RW_AspectTypeHandle = new Unity.Transforms.TransformAspect.TypeHandle(ref state, false);
        __Unity_Transforms_TransformAspect_RW_AspectLookup = new Unity.Transforms.TransformAspect.Lookup(ref state, false);
    }
}