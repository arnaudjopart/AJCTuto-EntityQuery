using System.Collections;
using System.Collections.Generic;
using Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class DebugDrawTargetLinesSystem : SystemBase
{
    protected override void OnUpdate()
    {
        
        Entities.WithAll<PlayerTagComponent>().ForEach((Entity _entity,in LocalToWorld _localToWorld) =>
        {
            var buffer = EntityManager.GetBuffer<TargetCollection>(_entity);
            foreach (var VARIABLE in buffer)
            {
                var targetTransform = GetComponent<LocalToWorld>(VARIABLE.m_entity);
                Debug.DrawLine(_localToWorld.Position,targetTransform.Position);
            }
            
        }).WithoutBurst().Run();
    }
}
