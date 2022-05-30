using Unity.Entities;
using Unity.Transforms;

namespace Systems
{

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

}


