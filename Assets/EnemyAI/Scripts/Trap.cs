using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("敵")]
    public GameObject enemyPre;

    Hole hole;

    public Transform[] spawnFarmPos;

    Renderer holeRenderer;

    Transform parent;

    void Start()
    {
        holeRenderer = GetComponent<Renderer>();
        hole = GetComponent<Hole>();
        holeRenderer.material = Resources.Load("Material/Hole01") as Material;

        parent = transform.parent.parent.parent.GetChild(4);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Minion") && hole.Infection == false)
        {
            BaseEnemy enemyCol = other.GetComponent<BaseEnemy>();

            if (enemyCol != null) return;

            if (transform.childCount != 0) Destroy(transform.GetChild(0).gameObject);

            for (int i = 0; i < spawnFarmPos.Length; i++)
            {
                if (spawnFarmPos[i].childCount != 0)
                {
                    Destroy(spawnFarmPos[i].GetChild(0).gameObject);

                    GameObject enemy = Instantiate(enemyPre, spawnFarmPos[i].position, Quaternion.identity);
                    enemy.transform.parent = parent;

                    enemy.GetComponent<BaseVegetable>().NearTarget = other.gameObject;
                }
            }
            
        }
    }
}

