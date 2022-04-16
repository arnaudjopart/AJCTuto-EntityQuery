using System.Collections;
using System.Collections.Generic;
using Systems;
using Unity.Entities;
using UnityEngine;

public class WeaponDataConversion : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{

    public float m_fireRate;
    public GameObject m_projectileGameObjectPrefab;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new FireWeaponComponent
        {
            m_currentRate = m_fireRate,
            m_fireRate = m_fireRate,
            m_projectileEntityPrefab = conversionSystem.GetPrimaryEntity(m_projectileGameObjectPrefab)
        });
  
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(m_projectileGameObjectPrefab);
    }
}
