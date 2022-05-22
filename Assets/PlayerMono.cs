using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using Unity.Transforms;
using UnityEngine;

public class PlayerMono : MonoBehaviour
{
    public GameObject m_prefab;

    public float m_fireRate;

    public EnemyContainer m_container;

    private double m_currentFireDelay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<Transform> enemies = GetEnemyList(m_container.listOfEnemies);
        m_currentFireDelay -= Time.deltaTime;
        if (m_currentFireDelay <= 0)
        {
            m_currentFireDelay = m_fireRate;
           

            foreach (var VARIABLE in enemies)
            {
                var item = Instantiate(m_prefab, transform.position + Vector3.up * .5f, Quaternion.identity);
                item.GetComponent<Projectile>().SetTarget(VARIABLE);
            }
            
        }
    }

    private List<Transform> GetEnemyList(List<MoveToTarget> _containerListOfEnemies)
    {
        var result = new List<Transform>();
        foreach (var VARIABLE in _containerListOfEnemies)
        {
            if(VARIABLE == null) continue;
            if (Vector3.Distance(transform.position, VARIABLE.transform.position) <= 5)
            {
                result.Add(VARIABLE.transform);
            }
        }

        return result;
    }
}