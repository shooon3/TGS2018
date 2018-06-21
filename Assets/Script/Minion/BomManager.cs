using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomManager : MonoBehaviour {

    /// <summary>
    /// ボムが何かにあたったかどうか
    /// </summary>
    public bool IsCollision { get; set; }

    public bool IsAttackBoss { get; set; }

    /// <summary>
    /// 衝突したボスオブジェクト
    /// </summary>
    public GameObject ColBossObj { get; set; }

    // Use this for initialization
    void Start ()
    {
       
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider col)
    {
        IsCollision = true;

        BaseBossEnemy boss = col.GetComponent<BaseBossEnemy>();

        if (boss != null)
        {
            IsAttackBoss = true;
            ColBossObj = col.gameObject;
        }

        //オブジェクトを消す
        Destroy(gameObject,0.2f);
        Destroy(GameObject.Find("Shadow(Clone)"));

    }
    
}
