using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radish : MonoBehaviour {

    private GameObject nearObj; //最も近いオブジェクト
    private float searchTime = 0; //経過時間

    // Use this for initialization
    void Start()
    {
        nearObj = serchTag(gameObject, "Player");
    }

    void Update()
    {
        searchTime += Time.deltaTime; //経過時間の取得

        if (searchTime >= 1.0f)
        {
            nearObj = serchTag(gameObject, "Player"); //最も近いオブジェクトを取得

            searchTime = 0; //経過時間を初期化
        }

        GameObject serchTag(GameObject nowObj,string tagname)
        {


        }
    }

    void OnCollisionStay(Collider col) //Playerの状態を検知 
    {
        if (col.tag == "Player")
        {
            //playerがいなければHoleへ戻る
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
