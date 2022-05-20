using System.Collections;
using System.Collections.Generic;
using Systems;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class MoveTowardTargetSystem : SystemBase
{
    
    protected override void OnUpdate()
    {
       /* var translations = GetComponentDataFromEntity<LocalToWorld>(true);

        Entities.WithReadOnly(translations).ForEach((ref MoveDirectionComponent _moveDirectionComponent, in TargetComponent _target, in Translation _translation) =>
        {
            var targetPosition = translations[_target.m_entity];
            var direction = math.normalize(targetPosition.Position - _translation.Value);
            //_moveDirectionComponent.m_value = direction;
        }).ScheduleParallel();*/
    }
}

[GenerateAuthoringComponent]
public struct MoveDirectionComponent : IComponentData
{
    public float3 m_value;
}