using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato : MonoBehaviour
{
    private GameObject nearObj;
    private float serchTime = 0;

    // Use this for initialization

    void OnCollisionStay(Collider col) //Playerの状態を検知
    {
        if (col.tag == "Player")
        {
            //PlayerがいなければHoleへ戻る
        }
       nearObj = serchTag(gameObject, "Player"); 

    }

    protected void attackInstantiate(GameObject hitObject, GameObject hitOffset)
    {
        GameObject hit;

        hit = Instantiate(hitObject, hitOffset.transform.position, transform.rotation) as GameObject;
        hit.transform.parent = hitOffset.transform;
        hit.GetComponent<EnemyStatus>().pc = this;
    }

    private GameObject serchTag(GameObject gameObject, string v)
    {
        throw new NotImplementedException();
    }

    void OnCollisionEnter(Collider col)  //Holeに戻ったらDestroy
    {
        if (col.gameObject.CompareTag("Hole"))
        {
            Destroy(col.gameObject);
        }
    }
}

