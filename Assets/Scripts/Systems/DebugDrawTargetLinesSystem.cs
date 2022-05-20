using System.Collections;
using System.Collections.Generic;
using Components;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Transforms;
using UnityEngine;

public partial class DebugDrawTargetLinesSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var lookup = GetBufferFromEntity<TargetCollection>();
        
        Entities.WithAll<PlayerTagComponent, MultiTargetTagComponent>().ForEach((Entity _entity,in LocalToWorld _localToWorld) =>
        {
            var buffer = lookup[_entity];
            foreach (var VARIABLE in buffer)
            {
                if (EntityManager.HasComponent<LocalToWorld>(VARIABLE.m_entity) == false) return;
                var targetTransform = GetComponent<LocalToWorld>(VARIABLE.m_entity);
                Debug.DrawLine(_localToWorld.Position,targetTransform.Position);
            }
            
        }).WithoutBurst().Run();
    }
}
