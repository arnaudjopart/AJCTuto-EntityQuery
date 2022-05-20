using System.Net;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial class CheckForReachingTargetSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_ecbs;

        protected override void OnCreate()
        {
            m_ecbs = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = m_ecbs.CreateCommandBuffer().AsParallelWriter();
            var lookup = GetComponentDataFromEntity<LocalToWorld>();
            Entities.WithReadOnly(lookup).ForEach(
                (Entity _entity, int entityInQueryIndex, in LocalToWorld _localToWorld,
                    in TargetComponent _targetComponent) =>
                {
                    if (_targetComponent.m_entity == Entity.Null) return;
                    
                    var targetCurrentPosition = lookup[_targetComponent.m_entity].Position;
                    
                    if (!(math.distancesq(_localToWorld.Position, targetCurrentPosition) < .5f)) return;

                    ecb.SetComponent(entityInQueryIndex,_entity,new TargetComponent()
                    {
                        m_entity = Entity.Null
                    });
                    
                    ecb.DestroyEntity(entityInQueryIndex, _entity);
                    ecb.DestroyEntity(entityInQueryIndex, _targetComponent.m_entity);
                    
                }).WithDisposeOnCompletion(lookup).ScheduleParallel();
            m_ecbs.AddJobHandleForProducer(Dependency);
        }
    }
}
