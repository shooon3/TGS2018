/*
 * PumpAIクラス
 * 味方AIを制御するクラス
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpAI : BaseVegetable
{

    //-----------------------------------------
    // public
    //-----------------------------------------

    public VegetableStatus status;

    //-----------------------------------------
    // private
    //-----------------------------------------

    //近いターゲットを格納する変数
    SerchNearObj serchTarget;

    //近い穴の位置を格納する変数
    GameObject nearHole;

    //近い敵の位置を格納する変数
    GameObject nearEnemy;

    //移動するポイント
    GameObject moveTarget;

    //親の位置を取得する変数
    Vector3 parentPos;

    //デストロイするオブジェクトの変数
    BaseVegetable target;

    Hole hole;

    //エネミーに当たった
    bool isEnemyCollision = false;

    //穴に当たった
    bool isHoleCollision = false;

    //-----------------------------------------
    // 関数
    //-----------------------------------------

    protected override void DoStart()
    {
        SetValue();

        SerchTarget();
    }

    protected override void DoUpdate()
    {
        ActionState();
    }

    /// <summary>
    /// 値を初期化
    /// </summary>
    void SetValue()
    {
        serchTarget = GetComponent<SerchNearObj>();

        //親のオブジェクトの位置を取得
        parentPos = transform.parent.transform.position;

        HP = status.hp;
        POW = status.pow;
        attackInterval = status.attackInterval;

        intervalTimer = attackInterval;
    }

    /// <summary>
    /// ターゲットを取得
    /// </summary>
    void SerchTarget()
    {
        //親のオブジェクトとHoleタグを探す
        nearHole = serchTarget.serchTag(parentPos, "Hole");

        nearEnemy = serchTarget.serchTag(parentPos, "Enemy");

        //近い穴の距離を取得
        Vector3 nearHolePos = nearHole.transform.position;

        //敵が存在していたら
        if (nearEnemy != null)
        {
            //敵の位置を取得
            Vector3 nearEnemyPos = nearEnemy.transform.position;

            //穴と自分の距離を測る
            float holeDis = Vector3.Distance(transform.position, nearHolePos);
            //敵と自分の距離をはかる
            float enemyDis = Vector3.Distance(transform.position, nearEnemyPos);

            //二つを比べて近いほうをNearTarget(攻撃対象)とする
            if (holeDis <= enemyDis) NearTarget = nearHole;
            else if (holeDis > enemyDis) NearTarget = nearEnemy;
        }
        //敵が存在していなかったら近い穴へ
        else NearTarget = nearHole;
    }

    /// <summary>
    /// 感染と敵の攻撃処理を分ける
    /// </summary>
    void ActionState()
    {
        if (isEnemyCollision) Attack();
        if (isHoleCollision) HoleInfection();
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    protected override void Attack()
    {
        if(IsAttack())
        {
            AddDamage(NearTarget);
        }
    }

    /// <summary>
    /// 感染させる
    /// </summary>
    void HoleInfection()
    {
        if (hole != null)
        {
            hole.Infectious();

            //畑が感染したらパンプ菌も消える
            if (hole.Infection) Destroy(transform.root.gameObject);
        }
    }


    void OnTriggerEnter(Collider col)
    {
        if (target != null || hole != null) return;

        //ターゲットにダメージを与える
        target = col.GetComponent<BaseVegetable>();

        //ターゲットにHoleスクリプトがアタッチされていたら
        hole = col.GetComponent<Hole>();


        if (target != null) isEnemyCollision = true;
        else if (hole != null) isHoleCollision = true;
    }

    /// <summary>
    /// Enemyが死ぬかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsDestroyEnemy()
    {
        if (HP <= 0 || NearTarget == null) return true;
        else return false;
    }
}