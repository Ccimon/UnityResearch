using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class TankAuthoring : MonoBehaviour
{

}

class TankBaker : Baker<TankAuthoring>
{
    public override void Bake(TankAuthoring authoring)
    {
        AddComponent<Tank>();
    }
}