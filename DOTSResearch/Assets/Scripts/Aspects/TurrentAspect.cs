using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

readonly  partial struct TurrentAspect : IAspect
{
    readonly RefRO<Turrent> m_Turrent;

    public Entity CannonBallSpawn => m_Turrent.ValueRO.CannonballSpawn;
    public Entity CannonBallPrefab => m_Turrent.ValueRO.ConnonballPrefab;
}
