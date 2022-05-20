using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public partial class DetectNearestPlayerSystem : SystemBase
    {
        private EntityQuery m_query;
        private EndSimulationEntityCommandBufferSystem m_ecbs;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_ecbs = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }
        protected override void OnUpdate()
        {
            var ecb = m_ecbs.CreateCommandBuffer().AsParallelWriter();
        
            m_query = GetEntityQuery(ComponentType.ReadOnly<PlayerTagComponent_Run>(),ComponentType.Exclude<WillBeDestroyedComponent>(), 
                ComponentType.ReadOnly<LocalToWorld>());
            var translationArray = m_query.ToComponentDataArray<LocalToWorld>(Allocator.TempJob);
            var entityArray = m_query.ToEntityArray(Allocator.Temp);
        
            Entities
                .WithReadOnly(translationArray)
                .WithReadOnly(entityArray)
                .WithAll<EnemyTagComponent>()
                .ForEach((Entity _entity, int entityInQueryIndex,  in Translation _translation) =>
                {
                    var dynamicBuffer = ecb.AddBuffer<TargetCollection>(entityInQueryIndex, _entity);
                    dynamicBuffer.Clear();
                    
                    var closestPlayer = FindClosestEntity(translationArray, entityArray, _translation);
                    if(closestPlayer!=Entity.Null) ecb.AppendToBuffer( entityInQueryIndex, _entity, new TargetCollection {m_entity = closestPlayer});
                })
                .WithDisposeOnCompletion(translationArray)
                .WithDisposeOnCompletion(entityArray)
                .ScheduleParallel();
        
            m_ecbs.AddJobHandleForProducer(Dependency);
        }

        private static Entity FindClosestEntity(NativeArray<LocalToWorld> _translationArray, NativeArray<Entity> _entityArray, Translation _translation)
        {
            if (_entityArray.Length == 0) return Entity.Null;
            var possibleClosestPlayer = _entityArray[0];
            var currentSmallestDistance = math.distancesq(_translationArray[0].Position, _translation.Value);
            
            for (var i = 1; i < _translationArray.Length; i++)
            {
                var currentDistance = math.distancesq(_translationArray[i].Position, _translation.Value);
                if (currentDistance > currentSmallestDistance) continue;
                currentSmallestDistance = currentDistance;
                possibleClosestPlayer = _entityArray[i];
                        
            }

            return possibleClosestPlayer;
        }
    }
}
