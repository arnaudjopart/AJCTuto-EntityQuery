using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Transforms;
using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
    public Transform[] m_players;
    [HideInInspector]
    public List<MoveToTarget> listOfEnemies = new List<MoveToTarget>();
    // Start is called before the first frame update
    void Start()
    {
        
        listOfEnemies = transform.GetComponentsInChildren<MoveToTarget>().ToList();
        foreach (var VARIABLE in listOfEnemies)
        {

            VARIABLE.FindNearestTarget(m_players);
        }
    }

    // Update is called once per frame
    void Update()
    {
        listOfEnemies = transform.GetComponentsInChildren<MoveToTarget>().ToList();
        Debug.Log(listOfEnemies.Count);
    }
}