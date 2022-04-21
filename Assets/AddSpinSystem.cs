using Unity.Entities;
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
            Entities.WithStructuralChanges().WithAll<SpinData>().ForEach((Entity _entity) =>
            {
                EntityManager.AddComponentData(_entity, new LifeTime()
                {
                    m_value = 4
                });
            }).Run();
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Entities.WithStructuralChanges().WithAll<SpinData>().ForEach((Entity _entity) =>
            {
                EntityManager.DestroyEntity(_entity);
            }).Run();
        }
        
        
    }
}


public struct LifeTime : IComponentData
{
    public float m_value;
}
