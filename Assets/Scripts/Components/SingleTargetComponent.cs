using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct SingleTargetComponent : IComponentData
    {
        public Entity m_target;
    }
}
