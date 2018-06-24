using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerchNearObj : MonoBehaviour
{
    public int AliveCount { get; set; }

    public int ChildAliveCount { get; set; }
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
    /// 子オブジェクトの中からタグを検索
    /// </summary>
    /// <param name="nowPos"></param>
    /// <param name="parent"></param>
    /// <param name="tagName"></param>
    /// <returns></returns>
    public GameObject serchChildTag(Vector3 nowPos, Transform parent, string tagName,bool isMoreChild = false)
    {
        float posDis = 0;  //一時変数
        float nearDis = 0; //最も近いオブジェクトの距離

        GameObject targetObj = null;

        List<GameObject> serchLis = new List<GameObject>();

        if (isMoreChild == true)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                foreach (Transform ob in parent.GetChild(i))
                {
                    if (ob.tag == tagName) serchLis.Add(ob.gameObject);
                }
            }
        }
        else
        {
            foreach (Transform ob in parent)
            {
                if (ob.tag == tagName) serchLis.Add(ob.gameObject);
            }
        }

        ChildAliveCount = serchLis.Count;

        //タグ指定されたオブジェクトを配列で取得
        foreach (GameObject obs in serchLis)
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

    /// <summary>
    /// 探したオブジェクトが１体でも残っているかどうか
    /// </summary>
    /// <param name="tagName"></param>
    /// <returns></returns>
    public bool IsTargetAliveChild(Transform parent, string tagName)
    {
        List<GameObject> serchLis = new List<GameObject>();

        foreach (Transform obj in parent)
        {
            if (obj.tag == tagName) serchLis.Add(obj.gameObject);
        }

        if (serchLis.Count == 0) return false;
        return true;
    }
}