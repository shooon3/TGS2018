using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PumpkinMove : MonoBehaviour {

    NavMeshAgent agent;

    //SerchHolesスクリプトが取得した位置を取得するための変数
    SerchNearObj serchNearObj;

    //近い穴の位置を格納する変数
    GameObject nearHole;

    //近い敵の位置を格納する変数
    GameObject nearEnemy;

    //移動するポイント
    GameObject moveTarget;

    //親の位置を取得する変数
    Vector3 parentPos;

	// Use this for initialization
	void Start () {

        agent = GetComponent<NavMeshAgent>();

        //SerchHolesスクリプトを取得
        serchNearObj = gameObject.GetComponentInParent<SerchNearObj>();

        //親のオブジェクトの位置を取得
        parentPos = transform.parent.transform.position;

        SerchMovePoint();
    }

	// Update is called once per frame
	void Update () {

        agent.destination = moveTarget.transform.position;
    }

    /// <summary>
    /// 移動する場所を決める
    /// </summary>
    void SerchMovePoint()
    {
        //親のオブジェクトとHoleタグを探す
        nearHole = serchNearObj.serchTag(parentPos, "Hole");

        nearEnemy = serchNearObj.serchTag(parentPos, "Enemy");

        Vector3 nearHolePos = nearHole.transform.position;

        //近くに敵がいた場合は穴と敵を比べて、近いほうに行く
        if (nearEnemy != null)
        {
            Vector3 nearEnemyPos = nearEnemy.transform.position;

            float holeDis = Vector3.Distance(transform.position, nearHolePos);
            float enemyDis = Vector3.Distance(transform.position, nearEnemyPos);

            if (holeDis <= enemyDis) moveTarget = nearHole;
            else if (holeDis > enemyDis) moveTarget = nearEnemy;
        }
        //いない場合は穴に行く
        else moveTarget = nearHole;
    }
}
