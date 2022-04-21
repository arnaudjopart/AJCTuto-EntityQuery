using Unity.Entities;

public partial class DeleteCubeSystem : SystemBase
{
    
    protected override void OnUpdate()
    {
        
        Entities.WithStructuralChanges().WithAll<SpinData,LifeTime>().ForEach((Entity _entity, in LifeTime _lifeTime) =>
        {
            if (_lifeTime.m_value <= 0)
            {
                EntityManager.RemoveComponent<SpinData>(_entity);
                EntityManager.RemoveComponent<LifeTime>(_entity);
            }

        }).Run();
    }
}