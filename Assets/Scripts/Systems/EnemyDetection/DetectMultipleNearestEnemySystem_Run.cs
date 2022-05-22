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
            var enemyEntityQuery = GetEntityQuery(typeof(EnemyTagComponent), ComponentType.ReadOnly<LocalToWorld>(), ComponentType.Exclude<AlreadyTargeted>());
            var positions = enemyEntityQuery.ToComponentDataArray<LocalToWorld>(Allocator.Temp);
            var targetEntityArray = enemyEntityQuery.ToEntityArray(Allocator.Temp);
            
            Entities.WithoutBurst().WithAll<MultiTargetTagComponent, MainThreadDetectionModeTag>().ForEach(
                (Entity _entity, in LocalToWorld _localToWorld) =>
                {
                    var buffer = EntityManager.GetBuffer<TargetCollection>(_entity);
                    buffer.Clear();
                    for (var i = 0; i < positions.Length; i++)
                    {
                        if (math.distancesq(positions[i].Position, _localToWorld.Position) > 25) continue;

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
    }
}
