using System.Collections;
using System.Collections.Generic;
using System.Net;
using Components;
using Systems;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(TestGroup))]
[UpdateAfter(typeof(MoveSystem))]
public partial class DetectMultipleNearestEnemiesSystem_Schedule : SystemBase
{
    private EntityQuery m_query;
    private EndSimulationEntityCommandBufferSystem m_ecbs;

    protected override void OnCreate()
    {
        base.OnCreate();
        m_ecbs = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var ecb = m_ecbs.CreateCommandBuffer().AsParallelWriter();
        
        m_query = GetEntityQuery(ComponentType.ReadOnly<EnemyTagComponent>(), 
            ComponentType.ReadOnly<Translation>(),ComponentType.Exclude<AlreadyTargeted>());
        var translationArray = m_query.ToComponentDataArray<Translation>(Allocator.TempJob);
        var entityArray = m_query.ToEntityArray(Allocator.TempJob);
        
        Entities
            .WithReadOnly(translationArray)
            .WithReadOnly(entityArray)
            .WithAll<MultiTargetTagComponent, ScheduleDetectionTag>()
            .ForEach((Entity _entity, int entityInQueryIndex, in Translation _translation) =>
            {
                ecb.AddBuffer<TargetCollection>(entityInQueryIndex, _entity);
     
                for (var i = 0; i < translationArray.Length; i++)
                {
                    if (!(math.distancesq(translationArray[i].Value, _translation.Value) < 25)) continue;
                    
                    ecb.AppendToBuffer( entityInQueryIndex, _entity, new TargetCollection {m_entity = entityArray[i]});
                }
            })
            .WithDisposeOnCompletion(translationArray)
            .WithDisposeOnCompletion(entityArray)
            .ScheduleParallel();
        
        m_ecbs.AddJobHandleForProducer(this.Dependency);

    }
}

public struct WillBeDestroyedComponent : IComponentData
{
}
