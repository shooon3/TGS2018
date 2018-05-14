using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : MonoBehaviour {

    public bool CreateFlg { get; set; }
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider col)
    {
        //衝突したオブジェクトがカボチャ爆弾だったら
        BomManager bomMar = col.GetComponent<BomManager>();
        if (bomMar == null) return;

        CreateFlg = true;
    }

}
