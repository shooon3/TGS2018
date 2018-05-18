﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public GameObject enemyPre;

    public Transform spawnFarmPos;

    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Minion"))
        {
            GameObject enemy = Instantiate(enemyPre, spawnFarmPos.position, Quaternion.identity);

            enemy.GetComponent<BaseVegetable>().NearTarget = other.gameObject;
            
        }
    }
}

