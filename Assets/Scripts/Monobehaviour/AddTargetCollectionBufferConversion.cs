using System.Collections;
using System.Collections.Generic;
using Components;
using Unity.Entities;
using UnityEngine;

public class AddTargetCollectionBufferConversion : MonoBehaviour, IConvertGameObjectToEntity
{
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddBuffer<TargetCollection>(entity);
    }
}
