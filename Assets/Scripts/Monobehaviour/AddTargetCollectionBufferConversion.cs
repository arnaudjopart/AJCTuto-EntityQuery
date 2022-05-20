using System.Collections;
using System.Collections.Generic;
using Components;
using Unity.Entities;
using UnityEngine;

public class AddTargetCollectionBufferConversion : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity _entity, EntityManager _dstManager, GameObjectConversionSystem _conversionSystem)
    {
        _dstManager.AddBuffer<TargetCollection>(_entity);
    }
}
