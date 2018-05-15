using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusDestoroy : MonoBehaviour {

    [SerializeField, Header("消滅するまでの時間")]
    private float dethTime;

	// Use this for initialization
	void Start () {
        
	}

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        //プレイヤータグを検知
        if (other.gameObject.tag == "Player")
        {
            //パンプ菌を削除
            Destroy(other.gameObject, dethTime);
        }
    }
}
