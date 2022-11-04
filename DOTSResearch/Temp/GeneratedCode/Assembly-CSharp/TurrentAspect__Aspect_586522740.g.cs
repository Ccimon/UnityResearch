using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


    readonly partial struct TurrentAspect : global::Unity.Entities.IAspect, global::Unity.Entities.IAspectCreate<TurrentAspect>
    {
        TurrentAspect(global::Unity.Entities.RefRO<global::Turrent> m_turrent)
        {
            this.m_Turrent = m_turrent;


        }
        public TurrentAspect CreateAspect(global::Unity.Entities.Entity entity, ref global::Unity.Entities.SystemState systemState, bool isReadOnly)
        {
            var lookup = new Lookup(ref systemState, isReadOnly);
            return lookup[entity];
        }

        public static global::Unity.Entities.ComponentType[] ExcludeComponents => global::System.Array.Empty<Unity.Entities.ComponentType>();
        static global::Unity.Entities.ComponentType[] s_RequiredComponents => new [] {  global::Unity.Entities.ComponentType.ReadOnly<global::Turrent>() };
        static global::Unity.Entities.ComponentType[] s_RequiredComponentsRO => new [] {  global::Unity.Entities.ComponentType.ReadOnly<global::Turrent>() };
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

            [global::Unity.Collections.ReadOnly]
            global::Unity.Entities.ComponentLookup<global::Turrent> m_TurrentComponentLookup;



            public Lookup(ref global::Unity.Entities.SystemState state, bool isReadOnly)
            {
                __IsReadOnly = isReadOnly ? (byte) 1u : (byte) 0u;
                this.m_TurrentComponentLookup = state.GetComponentLookup<global::Turrent>(true);



            }
            public void Update(ref global::Unity.Entities.SystemState state)
            {
                this.m_TurrentComponentLookup.Update(ref state);


            }
            public TurrentAspect this[global::Unity.Entities.Entity entity]
            {
                get
                {
                    return new TurrentAspect(this.m_TurrentComponentLookup.GetRefRO(entity));
                }
            }
        }
        public struct ResolvedChunk
        {

            internal global::Unity.Collections.NativeArray<global::Turrent> m_Turrent;


            public TurrentAspect this[int index]
            {
                get
                {
                    return new TurrentAspect(                        new global::Unity.Entities.RefRO<Turrent>(this.m_Turrent, index));
                }
            }
            public int Length;
        }
        public struct TypeHandle
        {
            [global::Unity.Collections.ReadOnly]
            global::Unity.Entities.ComponentTypeHandle<global::Turrent> m_TurrentCth;




            public TypeHandle(ref global::Unity.Entities.SystemState state, bool isReadOnly)
            {
                this.m_TurrentCth = state.GetComponentTypeHandle<global::Turrent>(true);




            }
            public void Update(ref global::Unity.Entities.SystemState state)
            {
                this.m_TurrentCth.Update(ref state);



            }
            public ResolvedChunk Resolve(global::Unity.Entities.ArchetypeChunk chunk)
            {
                ResolvedChunk resolved;


                resolved.m_Turrent = chunk.GetNativeArray(this.m_TurrentCth);

                resolved.Length = chunk.Count;
                return resolved;
            }
        }
        public static Enumerator Query(global::Unity.Entities.EntityQuery query, TypeHandle typeHandle) { return new Enumerator(query, typeHandle); }
        public struct Enumerator : global::System.Collections.Generic.IEnumerator<TurrentAspect>, global::System.Collections.Generic.IEnumerable<TurrentAspect>
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
            public TurrentAspect Current {
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
            global::System.Collections.Generic.IEnumerator<TurrentAspect> global::System.Collections.Generic.IEnumerable<TurrentAspect>.GetEnumerator() => throw new global::System.NotImplementedException();
            global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()=> throw new global::System.NotImplementedException();
        }

        /// <summary>
        /// Completes the dependency chain required for this aspect to have read access.
        /// So it completes all write dependencies of the components, buffers, etc. to allow for reading.
        /// </summary>
        /// <param name="state">The <see cref="SystemState"/> containing an <see cref="EntityManager"/> storing all dependencies.</param>
        public static void CompleteDependencyBeforeRO(ref global::Unity.Entities.SystemState state){
           state.EntityManager.CompleteDependencyBeforeRO<global::Turrent>();
        }

        /// <summary>
        /// Completes the dependency chain required for this component to have read and write access.
        /// So it completes all write dependencies of the components, buffers, etc. to allow for reading,
        /// and it completes all read dependencies, so we can write to it.
        /// </summary>
        /// <param name="state">The <see cref="SystemState"/> containing an <see cref="EntityManager"/> storing all dependencies.</param>
        public static void CompleteDependencyBeforeRW(ref global::Unity.Entities.SystemState state){
           state.EntityManager.CompleteDependencyBeforeRO<global::Turrent>();
        }
    }
