using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : MonoBehaviour {

    /// <summary>
    /// 爆弾が地面に衝突したら、パンプ菌を生成する
    /// </summary>
    public bool IsMinionCreate { get; set; }

    //１回だけアタッチする
    bool isFirstAttach = true;

    int childCount;

    PumpAI pumpAI;

    //死んだかどうかを判定するフラグ
    bool isDie;

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

            //自分の当たり判定を削除する
            Destroy(GetComponent<BoxCollider>());

            pumpAI = transform.GetChild(0).GetComponent<PumpAI>();

            isFirstAttach = false;
        }

        StartCoroutine(Die());

    }

    /// <summary>
    /// HPがなくなっていたらパンプキンを消す
    /// </summary>
    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.5f);

        //子オブジェクトが生成されていなかったら実行しない
        if (isFirstAttach != false) yield break;

        if (pumpAI.IsDestroyEnemy())
        {
            Destroy(gameObject);
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
