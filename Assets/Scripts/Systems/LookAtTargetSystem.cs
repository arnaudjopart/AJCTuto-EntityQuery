using System.Collections;
using System.Collections.Generic;
using Components;
using Systems;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class LookAtTargetSystem : SystemBase
{
    
    protected override void OnUpdate()
    {
        Entities.WithAll<EnemyTagComponent>().ForEach(
            (Entity _entity, ref Rotation _rotation, ref Translation _translation, in MoveDirectionComponent _moveDirection) =>
            {

                var quaternion = Unity.Mathematics.quaternion.LookRotation(_moveDirection.m_value, new float3(0, 1, 0));
                _rotation.Value = quaternion;

            }).ScheduleParallel();
    }
}
 