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
        var lookup = GetBufferFromEntity<TargetCollection>();
        
        Entities.WithAny<PlayerTagComponent, PlayerTagComponent_Run>().ForEach((Entity _entity,in LocalToWorld _localToWorld) =>
        {
            var buffer = lookup[_entity];
            foreach (var VARIABLE in buffer)
            {
                var targetTransform = GetComponent<LocalToWorld>(VARIABLE.m_entity);
                Debug.DrawLine(_localToWorld.Position,targetTransform.Position);
            }
            
        }).WithoutBurst().Run();
    }
}
