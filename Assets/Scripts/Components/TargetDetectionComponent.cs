using Unity.Entities;

[GenerateAuthoringComponent]
public struct TargetDetectionComponent : IComponentData
{
    public int m_nbReachableTargets;
}