using System.Collections;
using System.Collections.Generic;
using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class DetectNearestEnemySystem : SystemBase
{
    
    protected override void OnUpdate()
    {
        var targetEntityQuery = GetEntityQuery(ComponentType.ReadOnly<EnemyTagComponent>(), ComponentType.ReadOnly<LocalToWorld>());
        var targetPositionNativeArray = targetEntityQuery.ToComponentDataArray<LocalToWorld>(Allocator.TempJob);
        var targetLayerNativeArray = targetEntityQuery.ToComponentDataArray<EnemyTagComponent>(Allocator.TempJob);
        
        FindTarget(targetPositionNativeArray,targetLayerNativeArray);
        
    }

    private void FindTarget(NativeArray<LocalToWorld> _targetsPositionArray,
        NativeArray<EnemyTagComponent> _targetLayerNativeArray)
    {
        Entities.ForEach(
            (Entity _entity,  in LocalToWorld _localToWorld) =>
            {
                var nearestTarget = _targetsPositionArray[0];
            }).WithDisposeOnCompletion(_targetsPositionArray).ScheduleParallel();
    }
    /*
    protected override void OnUpdate()
    {
        var query = GetEntityQuery(ComponentType.ReadOnly<EnemyTagComponent>(), ComponentType.ReadOnly<LocalToWorld>());
        var translationArray = query.ToComponentDataArray<LocalToWorld>(Allocator.TempJob);
        
        Entities.WithAll<PlayerTagComponent>().ForEach((Entity _entity, ref Translation _translation) =>
        {
            for (var i = 0; i < translationArray.Length; i++)
            {
                var position = translationArray[i].Position;
                /*if (math.distancesq(translationArray[i].Position, _translation.Value) < 25)
                {
                    
                }
            }
        }).WithDisposeOnCompletion(translationArray).ScheduleParallel();
        

    }*/
}
