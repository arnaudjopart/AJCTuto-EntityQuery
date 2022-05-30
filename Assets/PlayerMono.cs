using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using Unity.Transforms;
using UnityEngine;

public class PlayerMono : MonoBehaviour
{
    public Projectile m_prefab;

    public float m_fireRate;

    public EnemyContainer m_container;

    private float m_currentFireDelay;
    
    // Update is called once per frame
    private void Update()
    {
        var detectedEnemies = GetEnemyList(m_container.m_listOfEnemies);
        m_currentFireDelay -= Time.deltaTime;
        if (!(m_currentFireDelay <= 0)) return;
        m_currentFireDelay = m_fireRate;
        
        foreach (var target in detectedEnemies)
        {
            var item = Instantiate(m_prefab, transform.position + Vector3.up * .5f, Quaternion.identity);
            item.SetTarget(target);
        }
    }

    private List<Transform> GetEnemyList(List<MoveToTarget> _containerListOfEnemies)
    {
        var result = new List<Transform>();
        foreach (var enemy in _containerListOfEnemies)
        {
            if (enemy.gameObject.activeSelf == false) continue;
            if (Vector3.Distance(transform.position, enemy.transform.position) <= 5)
            {
                result.Add(enemy.transform);
            }
        }

        return result;
    }
}