using Unity.Entities;

namespace Systems
{
    public struct FireWeaponComponent : IComponentData
    {
        public Entity m_projectileEntityPrefab;
        public float m_fireRate;
        public float m_currentRate;
    }
}