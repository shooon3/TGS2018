using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : MonoBehaviour {

    public bool CreateFlg { get; set; }

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
            parentCollider.transform.parent = transform.GetChild(1);
        }

	}

    void OnTriggerEnter(Collider col)
    {
        //衝突したオブジェクトがカボチャ爆弾だったら
        BomManager bomMar = col.GetComponent<BomManager>();
        if (bomMar == null) return;

        CreateFlg = true;
    }
}
