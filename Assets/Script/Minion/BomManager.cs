using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomManager : MonoBehaviour {

    /// <summary>
    /// ボムが何かにあたったかどうか
    /// </summary>
    public bool IsCollision { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(-5.0f, 0, 0, Space.World);
    }

    void OnTriggerEnter(Collider col)
    {
        ////パンプ菌生成位置にオブジェクトが到達したら
        //MinionManager minionMar = col.GetComponent<MinionManager>();
        //if(minionMar != null)
        //{
            IsCollision = true;
            //オブジェクトを消す
            Destroy(gameObject,0.5f);
        //}
    }
}
