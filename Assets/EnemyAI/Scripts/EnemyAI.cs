using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    GameObject nearObj;

    SerchNearObj serchNearObj;

    NavMeshAgent agent;

    private Transform MinionPre;

    private void Start()
    {
        // Navigationで追尾
        agent = GetComponent<NavMeshAgent>();

        serchNearObj = GetComponent<SerchNearObj>();

        nearObj = serchNearObj.serchTag(transform.position,"Minion");
    }

    private void Update()
    {
        if (nearObj != null)
        {
            // Playerの座標に移動
            agent.destination = nearObj.transform.position;
        }
    }
}
