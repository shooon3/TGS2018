using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(-5.0f, 0, 0, Space.World);
    }

    void OnTriggerEnter(Collider col)
    {
        //パンプ菌生成位置にオブジェクトが到達したら
        MinionManager minionMar = col.GetComponent<MinionManager>();
        if(minionMar != null)
        {

            //オブジェクトを消す
            Destroy(gameObject,0.5f);
        }
    }
}
