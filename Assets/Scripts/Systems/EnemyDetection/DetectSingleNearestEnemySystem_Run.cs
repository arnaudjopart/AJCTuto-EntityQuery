using System.Collections;
using System.Collections.Generic;
using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems.EnemyDetection
{
    public partial class DetectSingleNearestEnemySystem_Run : SystemBase
    {
        private EntityQueryDesc m_queryDescription;

        protected override void OnStartRunning()
        {
            m_queryDescription = new EntityQueryDesc
            {
                None = new ComponentType[] { typeof(AlreadyTargeted) },
                All = new[]{ typeof(EnemyTagComponent),ComponentType.ReadOnly<LocalToWorld>() }
            };
        }

        protected override void OnUpdate()
        {
            var entityQuery = GetEntityQuery(m_queryDescription);
        
            var positionNativeArray = entityQuery.ToComponentDataArray<LocalToWorld>(Allocator.Temp);
            var targetEntityArray = entityQuery.ToEntityArray(Allocator.Temp);
        
            Entities.WithoutBurst().ForEach((
                Entity _entity, 
                ref SingleTargetComponent _targetComponent,
                in LocalToWorld _localToWorld) =>
            {
                var possibleNearestEntityDistance = 25f;
                var possibleTarget = Entity.Null;
            
                for (var i = 0; i < positionNativeArray.Length; i++)
                {
                    var distance = math.distancesq(_localToWorld.Position, positionNativeArray[i].Position);
                    if (distance > 25) continue;
                    {
                        if(distance>possibleNearestEntityDistance) continue;
                        possibleNearestEntityDistance = distance;
                        possibleTarget = targetEntityArray[i];
                    }
                }

                if (possibleTarget == Entity.Null) return;
                _targetComponent.m_target = possibleTarget;
            

            }).WithDisposeOnCompletion(positionNativeArray).WithDisposeOnCompletion(targetEntityArray).Run();
        }
    }

    public struct AlreadyTargeted: IComponentData
    {
    }
}