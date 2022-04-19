using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class SpawnEntity : SystemBase
{

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            var entity = EntityManager.CreateEntity();
            EntityManager.AddComponentData(entity, new Translation
            {
                Value = float3.zero
            });
        }
    }
}
