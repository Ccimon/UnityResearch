using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class TurrentAuthoring : MonoBehaviour
{
    public GameObject CannonballPrefab;
    public Transform CannonballSpawn;
}

class TurrentBaker : Baker<TurrentAuthoring>
{
    public override void Bake(TurrentAuthoring authoring)
    {
        AddComponent<Turrent>(new Turrent()
        {
            CannonballSpawn = GetEntity(authoring.CannonballSpawn),
            ConnonballPrefab = GetEntity(authoring.CannonballPrefab)
        });
    }
}