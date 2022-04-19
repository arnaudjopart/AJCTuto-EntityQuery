using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class AddSpinSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem m_ecbs;

    protected override void OnCreate()
    {
        m_ecbs = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            var ecb = m_ecbs.CreateCommandBuffer().AsParallelWriter();
            
            Entities.WithAll<SpinCubeTag>().WithNone<SpinData>().ForEach((Entity _entity, int entityInQueryIndex) =>
            {
                ecb.AddComponent(entityInQueryIndex,_entity, new SpinData
                {
                    m_speed = 5
                });
            }).ScheduleParallel();
            
            m_ecbs.AddJobHandleForProducer(Dependency);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Entities.WithStructuralChanges().ForEach((Entity _entity) =>
            {
                EntityManager.AddComponentData(_entity, new LifeTime()
                {
                    m_value = 10
                });
            }).Run();
        }
        
    }
}


public partial class SpinSystem : SystemBase
{
    
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        Entities.ForEach((ref Rotation _rotation, in SpinData _spinData) =>
        {
            var nextRotation = math.mul(_rotation.Value, quaternion.RotateY(deltaTime*_spinData.m_speed));
            _rotation.Value = nextRotation;

        }).Run();
    }
}


public partial class DeleteCubeSystem : SystemBase
{
    
    protected override void OnUpdate()
    {
        
        Entities.WithStructuralChanges().ForEach((Entity _entity, in LifeTime _lifeTime) =>
        {
           if(_lifeTime.m_value<=0) EntityManager.DestroyEntity(_entity);

        }).Run();
    }
}
public partial class DecreaseLifeTimeSystem : SystemBase
{
    
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        Entities.ForEach((ref LifeTime _lifeTime) =>
        {
            
            _lifeTime.m_value -= deltaTime;
        }).Run();
    }
}


public struct LifeTime : IComponentData
{
    public float m_value;
}
