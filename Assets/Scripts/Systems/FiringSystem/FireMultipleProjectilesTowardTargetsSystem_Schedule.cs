using System.Net;
using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public partial class FireMultipleProjectilesTowardTargetsSystem_Schedule : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_ecbs;

        protected override void OnCreate()
        {
            m_ecbs = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = m_ecbs.CreateCommandBuffer().AsParallelWriter();
            var buffer = GetBufferFromEntity<TargetCollection>();
            var deltaTime = Time.DeltaTime;
            
            var localToWorldFromEntity = GetComponentDataFromEntity<LocalToWorld>(true);
            var alreadyTargetedList = GetComponentDataFromEntity<AlreadyTargeted>(true);

            Entities
                .WithReadOnly(localToWorldFromEntity)
                .WithReadOnly(alreadyTargetedList)
                .WithReadOnly(buffer)
                .WithAll<PlayerTagComponent, MultiTargetTagComponent>()
                .ForEach((Entity _entity, int entityInQueryIndex,ref FireWeaponComponent _weaponData, in Translation _translation) =>
            {
                var collection = buffer[_entity];
                if (_weaponData.m_currentRate > 0)
                {
                    _weaponData.m_currentRate -= deltaTime;
                    return;
                }

                _weaponData.m_currentRate = _weaponData.m_fireRate;
                
                for (var i = 0; i < collection.Length; i++)
                {
                    if (alreadyTargetedList.HasComponent(collection[i].m_entity)) continue;
                    
                    var projectile  = ecb.Instantiate(entityInQueryIndex, _weaponData.m_projectileEntityPrefab);
                    ecb.SetComponent(entityInQueryIndex,projectile, new Translation
                    {
                        Value = _translation.Value+new float3(0,.5f,0)
                    });
                    ecb.AddComponent(entityInQueryIndex,projectile, new TargetComponent
                    {
                        m_entity = collection[i].m_entity
                    });
                    var targetPosition = localToWorldFromEntity[collection[i].m_entity];
                    var direction = math.normalize(targetPosition.Position - _translation.Value);
                    
                    ecb.AddComponent(entityInQueryIndex, projectile, new MoveDirectionComponent
                    {
                        m_value = direction
                    });
                    
                    ecb.AddComponent(entityInQueryIndex, collection[i].m_entity, new AlreadyTargeted()
                    {
                    });
                    
                }
            }).ScheduleParallel();
            m_ecbs.AddJobHandleForProducer(Dependency);
        }
    }

    public struct TargetComponent: IComponentData
    {
        public Entity m_entity;
    }
}
