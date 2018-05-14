using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject target;
    NavMeshAgent agent;

    private Transform MinionPre;

    private void Start()
    {
        // Navigationで追尾
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Playerの座標に移動
        agent.destination = target.transform.position;
    }
}
