using System.Collections;
using System.Collections.Generic;
using Components;
using Systems;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class FireSingleProjectileSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        
        Entities.WithStructuralChanges().WithoutBurst().ForEach((
            Entity _entity, ref FireWeaponComponent _weaponData, ref SingleTargetComponent _singleTarget, in LocalToWorld _localToWorld) => {
            
            if (_weaponData.m_currentRate > 0)
            {
                _weaponData.m_currentRate -= deltaTime;
                return;
            }
            
            if (!EntityManager.Exists(_singleTarget.m_target)) return;

            _weaponData.m_currentRate = _weaponData.m_fireRate;

            var projectile = EntityManager.Instantiate(_weaponData.m_projectileEntityPrefab);
            
            EntityManager.SetComponentData(projectile, new Translation
            {
                Value = _localToWorld.Position + new float3(0, .5f, 0)
            });
            EntityManager.AddComponentData(projectile, new TargetComponent
            {
                m_entity = _singleTarget.m_target
            });
            var targetPosition = EntityManager.GetComponentData<LocalToWorld>(_singleTarget.m_target);
            EntityManager.AddComponentData(_singleTarget.m_target, new AlreadyTargeted());
            var direction = math.normalize(targetPosition.Position - _localToWorld.Position);

            EntityManager.AddComponentData(projectile, new MoveDirectionComponent
            {
                m_value = direction
            });
            _singleTarget.m_target = Entity.Null;
        }).Run();
    }
}
