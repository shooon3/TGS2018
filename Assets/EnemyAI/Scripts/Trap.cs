using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public GameObject enemy;

    public Transform spawnFarmPos;

    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Minion"))
        {
            Instantiate(enemy, spawnFarmPos.position, Quaternion.identity);
        }
    }
}

