using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct TargetCollection : IBufferElementData
    {
        public Entity m_entity;
    }
}