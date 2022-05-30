using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Transforms;
using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
    [SerializeField] private Transform[] m_players;
    [HideInInspector] public List<MoveToTarget> m_listOfEnemies = new List<MoveToTarget>();

    private void Start()
    {
        m_listOfEnemies = transform.GetComponentsInChildren<MoveToTarget>().ToList();
        foreach (var enemy in m_listOfEnemies)
        {
            enemy.FindNearestTarget(m_players);
        }
    }
}