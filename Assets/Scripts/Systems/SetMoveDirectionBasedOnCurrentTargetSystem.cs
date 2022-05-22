

using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public partial class SetMoveDirectionBasedOnCurrentTargetSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_ecbs;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_ecbs = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = m_ecbs.CreateCommandBuffer().AsParallelWriter();
            var buffer = GetBufferFromEntity<TargetCollection>();

        
            var localToWorldFromEntity = GetComponentDataFromEntity<LocalToWorld>(true);
            Entities.WithReadOnly(buffer).WithNone<MoveDirectionComponent>().WithReadOnly(localToWorldFromEntity).ForEach((Entity _entity, int entityInQueryIndex,in Translation _translation) =>
            {
                if (!buffer.HasComponent(_entity)) return;
                var dynamicBuffer = buffer[_entity];
                
                if (dynamicBuffer.Length <= 0) return;
                if (!localToWorldFromEntity.HasComponent(dynamicBuffer[0].m_entity)) return;
                
                var targetPosition = localToWorldFromEntity[dynamicBuffer[0].m_entity];
                var direction = math.normalize(targetPosition.Position - _translation.Value);
                ecb.AddComponent(entityInQueryIndex, _entity, new MoveDirectionComponent
                {
                    m_value = direction
                });
            }).WithDisposeOnCompletion(buffer).WithDisposeOnCompletion(localToWorldFromEntity).ScheduleParallel();
            m_ecbs.AddJobHandleForProducer(Dependency);
        }
    }
}
