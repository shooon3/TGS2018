using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public GameObject enemyPre;

    public Transform spawnFarmPos;

    Renderer holeRenderer;

    void Start()
    {
        holeRenderer = GetComponent<Renderer>();
        holeRenderer.material = Resources.Load("Material/Hole01") as Material;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Minion") && transform.childCount == 0)
        {
            GameObject enemy = Instantiate(enemyPre, spawnFarmPos.position, Quaternion.identity);
            enemy.transform.parent = transform;

            enemy.GetComponent<BaseVegetable>().NearTarget = other.gameObject;
            
        }
    }
}

