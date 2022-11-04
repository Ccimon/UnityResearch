using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


    readonly partial struct CannonBallAspect : global::Unity.Entities.IAspect, global::Unity.Entities.IAspectCreate<CannonBallAspect>
    {
        CannonBallAspect(global::Unity.Entities.Entity entity,global::Unity.Transforms.TransformAspect transform,global::Unity.Entities.RefRW<global::Cannonball> cannonball)
        {
            this.Transform = transform;
            this.CannonBall = cannonball;

            this.Self = entity;

        }
        public CannonBallAspect CreateAspect(global::Unity.Entities.Entity entity, ref global::Unity.Entities.SystemState systemState, bool isReadOnly)
        {
            var lookup = new Lookup(ref systemState, isReadOnly);
            return lookup[entity];
        }

        public static global::Unity.Entities.ComponentType[] ExcludeComponents => global::System.Array.Empty<Unity.Entities.ComponentType>();
        static global::Unity.Entities.ComponentType[] s_RequiredComponents => global::Unity.Entities.ComponentType.Combine(new [] {  global::Unity.Entities.ComponentType.ReadWrite<global::Cannonball>() },  Unity.Transforms.TransformAspect.RequiredComponents);
        static global::Unity.Entities.ComponentType[] s_RequiredComponentsRO => global::Unity.Entities.ComponentType.Combine(new [] {  global::Unity.Entities.ComponentType.ReadOnly<global::Cannonball>() },  Unity.Transforms.TransformAspect.RequiredComponentsRO);
        public static global::Unity.Entities.ComponentType[] RequiredComponents => s_RequiredComponents;
        public static global::Unity.Entities.ComponentType[] RequiredComponentsRO => s_RequiredComponentsRO;
        public struct Lookup
        {
            bool _IsReadOnly
            {
                get { return __IsReadOnly == 1; }
                set { __IsReadOnly = value ? (byte) 1 : (byte) 0; }
            }
            private byte __IsReadOnly;

            global::Unity.Entities.ComponentLookup<global::Cannonball> CannonBallComponentLookup;


            global::Unity.Transforms.TransformAspect.Lookup Transform;
            public Lookup(ref global::Unity.Entities.SystemState state, bool isReadOnly)
            {
                __IsReadOnly = isReadOnly ? (byte) 1u : (byte) 0u;
                this.CannonBallComponentLookup = state.GetComponentLookup<global::Cannonball>(isReadOnly);


                this.Transform = new global::Unity.Transforms.TransformAspect.Lookup(ref state, isReadOnly);
            }
            public void Update(ref global::Unity.Entities.SystemState state)
            {
                this.CannonBallComponentLookup.Update(ref state);

                this.Transform.Update(ref state);
            }
            public CannonBallAspect this[global::Unity.Entities.Entity entity]
            {
                get
                {
                    return new CannonBallAspect(entity,this.Transform[entity],this.CannonBallComponentLookup.GetRefRW(entity, _IsReadOnly));
                }
            }
        }
        public struct ResolvedChunk
        {
            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Entity> m_Entities;
            internal global::Unity.Collections.NativeArray<global::Cannonball> CannonBall;

            internal global::Unity.Transforms.TransformAspect.ResolvedChunk Transform;
            public CannonBallAspect this[int index]
            {
                get
                {
                    return new CannonBallAspect(m_Entities[index],
this.Transform[index],
                        new global::Unity.Entities.RefRW<Cannonball>(this.CannonBall, index));
                }
            }
            public int Length;
        }
        public struct TypeHandle
        {
            global::Unity.Entities.ComponentTypeHandle<global::Cannonball> CannonBallCth;

            global::Unity.Entities.EntityTypeHandle m_Entities;

            global::Unity.Transforms.TransformAspect.TypeHandle Transform;
            public TypeHandle(ref global::Unity.Entities.SystemState state, bool isReadOnly)
            {
                this.CannonBallCth = state.GetComponentTypeHandle<global::Cannonball>(isReadOnly);

                this.m_Entities = state.GetEntityTypeHandle();

                this.Transform = new global::Unity.Transforms.TransformAspect.TypeHandle(ref state, isReadOnly);
            }
            public void Update(ref global::Unity.Entities.SystemState state)
            {
                this.CannonBallCth.Update(ref state);

                this.m_Entities.Update(ref state);
                this.Transform.Update(ref state);
            }
            public ResolvedChunk Resolve(global::Unity.Entities.ArchetypeChunk chunk)
            {
                ResolvedChunk resolved;
                resolved.m_Entities = chunk.GetNativeArray(this.m_Entities);
                resolved.Transform = this.Transform.Resolve(chunk);
                resolved.CannonBall = chunk.GetNativeArray(this.CannonBallCth);

                resolved.Length = chunk.Count;
                return resolved;
            }
        }
        public static Enumerator Query(global::Unity.Entities.EntityQuery query, TypeHandle typeHandle) { return new Enumerator(query, typeHandle); }
        public struct Enumerator : global::System.Collections.Generic.IEnumerator<CannonBallAspect>, global::System.Collections.Generic.IEnumerable<CannonBallAspect>
        {
            ResolvedChunk                                _Resolved;
            global::Unity.Entities.EntityQueryEnumerator _QueryEnumerator;
            TypeHandle                                   _Handle;
            internal Enumerator(global::Unity.Entities.EntityQuery query, TypeHandle typeHandle)
            {
                _QueryEnumerator = new global::Unity.Entities.EntityQueryEnumerator(query);
                _Handle = typeHandle;
                _Resolved = default;
            }
            public void Dispose() { _QueryEnumerator.Dispose(); }
            public bool MoveNext()
            {
                if (_QueryEnumerator.MoveNextHotLoop())
                    return true;
                return MoveNextCold();
            }
            [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
            bool MoveNextCold()
            {
                var didMove = _QueryEnumerator.MoveNextColdLoop(out var chunk);
                if (didMove)
                    _Resolved = _Handle.Resolve(chunk);
                return didMove;
            }
            public CannonBallAspect Current {
                get {
                    #if ENABLE_UNITY_COLLECTIONS_CHECKS || UNITY_DOTS_DEBUG
                        _QueryEnumerator.CheckDisposed();
                    #endif
                        return _Resolved[_QueryEnumerator.IndexInChunk];
                    }
            }
            public Enumerator GetEnumerator()  { return this; }
            void global::System.Collections.IEnumerator.Reset() => throw new global::System.NotImplementedException();
            object global::System.Collections.IEnumerator.Current => throw new global::System.NotImplementedException();
            global::System.Collections.Generic.IEnumerator<CannonBallAspect> global::System.Collections.Generic.IEnumerable<CannonBallAspect>.GetEnumerator() => throw new global::System.NotImplementedException();
            global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()=> throw new global::System.NotImplementedException();
        }

        /// <summary>
        /// Completes the dependency chain required for this aspect to have read access.
        /// So it completes all write dependencies of the components, buffers, etc. to allow for reading.
        /// </summary>
        /// <param name="state">The <see cref="SystemState"/> containing an <see cref="EntityManager"/> storing all dependencies.</param>
        public static void CompleteDependencyBeforeRO(ref global::Unity.Entities.SystemState state){
           Unity.Transforms.TransformAspect.CompleteDependencyBeforeRW(ref state);
           state.EntityManager.CompleteDependencyBeforeRO<global::Cannonball>();
        }

        /// <summary>
        /// Completes the dependency chain required for this component to have read and write access.
        /// So it completes all write dependencies of the components, buffers, etc. to allow for reading,
        /// and it completes all read dependencies, so we can write to it.
        /// </summary>
        /// <param name="state">The <see cref="SystemState"/> containing an <see cref="EntityManager"/> storing all dependencies.</param>
        public static void CompleteDependencyBeforeRW(ref global::Unity.Entities.SystemState state){
           Unity.Transforms.TransformAspect.CompleteDependencyBeforeRO(ref state);
           state.EntityManager.CompleteDependencyBeforeRW<global::Cannonball>();
        }
    }
