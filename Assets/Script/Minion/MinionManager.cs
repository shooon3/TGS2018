using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : MonoBehaviour {

    /// <summary>
    /// 爆弾が地面に衝突したら、パンプ菌を生成する
    /// </summary>
    public bool IsMinionCreate { get; set; }

    bool isFirstAttach = true;

    int childCount;

	// Use this for initialization
	void Start () {

        childCount = transform.childCount;

    }
	
	// Update is called once per frame
	void Update () {
		
        //子オブジェクトが増えたら
        if(childCount < transform.childCount && isFirstAttach)
        {
            //菌の当たり判定を、一番最初に生成された菌に追従させる
            BoxCollider collider = transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;

            isFirstAttach = false;
        }

    }

    void OnTriggerEnter(Collider col)
    {
        //衝突したオブジェクトがカボチャ爆弾だったら
        BomManager bomMar = col.GetComponent<BomManager>();
        if (bomMar == null) return;

        IsMinionCreate = true;
    }
}
