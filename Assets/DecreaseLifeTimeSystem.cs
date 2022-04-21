using Unity.Entities;

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