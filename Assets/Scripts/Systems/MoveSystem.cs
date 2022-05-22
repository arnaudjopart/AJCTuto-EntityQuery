using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [UpdateInGroup(typeof(TestGroup))]
    public partial class MoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((ref Translation _translation, in MoveComponent _moveComponent,
                in MoveDirectionComponent _moveDirectionComponent) =>
            {
                _translation.Value += _moveDirectionComponent.m_value * _moveComponent.m_speed * deltaTime;
            }).ScheduleParallel();
            
            Dependency.Complete();
        }

        
    }
    /*
    public struct MoveJob : IJobEntityBatch
    {
        public ComponentTypeHandle<float3> velocityHandle
        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            throw new System.NotImplementedException();
        }
    }*/
}


[UpdateInGroup(typeof(SimulationSystemGroup))]
public class TestGroup : ComponentSystemGroup
{
    
}