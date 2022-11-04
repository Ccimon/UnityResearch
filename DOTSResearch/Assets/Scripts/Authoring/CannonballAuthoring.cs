using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CannonballAuthoring : MonoBehaviour
{
   
}

class CannonballBaker : Baker<CannonballAuthoring>
{
    public override void Bake(CannonballAuthoring authoring)
    {
        AddComponent<Cannonball>();
    }
}