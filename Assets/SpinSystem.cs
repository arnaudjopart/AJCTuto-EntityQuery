using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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