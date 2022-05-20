using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial class DetectNearestEnemySystem_Run : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            //Debug.Log("Start");
        }

        protected override void OnUpdate()
        {
            var enemyEntityQuery = GetEntityQuery(typeof(EnemyTagComponent), ComponentType.ReadOnly<LocalToWorld>());
            var positions = enemyEntityQuery.ToComponentDataArray<LocalToWorld>(Allocator.Temp);
            var targetEntityArray = enemyEntityQuery.ToEntityArray(Allocator.Temp);
            
            Entities.WithoutBurst().WithAll<PlayerTagComponent_Run>().ForEach(
                (Entity _entity, ref TargetDetectionComponent _targetDetection,in LocalToWorld _localToWorld) =>
                {
                    Debug.Log(positions.Length);
                    
                    _targetDetection.m_nbReachableTargets = 0;
                    var buffer = EntityManager.GetBuffer<TargetCollection>(_entity);
                    buffer.Clear();
                    for (var i = 0; i < positions.Length; i++)
                    {
                        if (math.distancesq(positions[i].Position, _localToWorld.Position) > 25) continue;
                        
                        _targetDetection.m_nbReachableTargets += 1;
                        buffer.Add(new TargetCollection
                        {
                            m_entity = targetEntityArray[i]

                        });
                    }
                })
                .WithDisposeOnCompletion(positions)
                .WithDisposeOnCompletion(targetEntityArray)
                .Run();
                
        }

        protected override void OnStopRunning()
        {
            base.OnStopRunning();
            Debug.Log("OnStopRunning");
        }
    }
}
