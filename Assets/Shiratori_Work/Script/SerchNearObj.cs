using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerchNearObj : MonoBehaviour
{
    public int AliveCount { get; set; }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject serchTag(Vector3 nowPos, string tagName)
    {
        float posDis = 0;  //一時変数
        float nearDis = 0; //最も近いオブジェクトの距離

        GameObject targetObj = null;

        AliveCount = GameObject.FindGameObjectsWithTag(tagName).Length;

        //タグ指定されたオブジェクトを配列で取得
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
        {
            //自身と取得したオブジェクトの距離を取得
            posDis = Vector3.Distance(obs.transform.position, nowPos);

            //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > posDis)
            {
                nearDis = posDis;
                targetObj = obs;
            }
        }
        return targetObj;
    }

    /// <summary>
    /// 探したオブジェクトが１体でも残っているかどうか
    /// </summary>
    /// <param name="tagName"></param>
    /// <returns></returns>
    public bool IsTargetAlive(string tagName)
    {
        if (GameObject.FindGameObjectWithTag(tagName) == null) return false; 
        return true;
    }
}