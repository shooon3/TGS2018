using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : MonoBehaviour {

    /// <summary>
    /// 爆弾が地面に衝突したら、パンプ菌を生成する
    /// </summary>
    public bool IsMinionCreate { get; set; }

    GameObject parentCollider;

    int childCount;

	// Use this for initialization
	void Start () {

        parentCollider = transform.GetChild(0).gameObject;

        childCount = transform.childCount;

    }
	
	// Update is called once per frame
	void Update () {
		
        //子オブジェクトが増えたら
        if(childCount < transform.childCount)
        {
            //菌の当たり判定を、一番最初に生成された菌に追従させる
            parentCollider.transform.parent = transform.GetChild(1);
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
