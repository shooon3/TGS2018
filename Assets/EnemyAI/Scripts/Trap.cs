using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("敵")]
    public GameObject enemyPre;

    //public GameObject farmEnemy;

    public Transform[] spawnFarmPos;

    Renderer holeRenderer;

    void Start()
    {
        holeRenderer = GetComponent<Renderer>();
        holeRenderer.material = Resources.Load("Material/Hole01") as Material;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Minion"))
        {
            if(transform.childCount != 0) Destroy(transform.GetChild(0).gameObject);

            for (int i = 0; i < spawnFarmPos.Length; i++)
            {
                if (spawnFarmPos[i].childCount != 0)
                {
                    Destroy(spawnFarmPos[i].GetChild(0).gameObject);

                    GameObject enemy = Instantiate(enemyPre, spawnFarmPos[i].position, Quaternion.identity);

                    enemy.GetComponent<BaseVegetable>().NearTarget = other.gameObject;
                }
            }
            
        }
    }
}

