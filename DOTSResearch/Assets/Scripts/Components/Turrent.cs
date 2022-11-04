using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

struct Turrent:IComponentData
{
    public Entity CannonballSpawn;
    public Entity ConnonballPrefab;
}
