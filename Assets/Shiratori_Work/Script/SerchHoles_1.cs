using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SerchHoles_1 : MonoBehaviour
{
    [SerializeField, Header("ホール 1")]
    private GameObject Hole_1;
    [SerializeField, Header("ホール 2")]
    private GameObject Hole_2;

    NavMeshAgent agent;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = Hole_1.transform.position;

        agent.destination = Hole_2.transform.position;
    }
}