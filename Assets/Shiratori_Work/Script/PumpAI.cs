﻿/*
 * PumpAIクラス
 * 味方AIを制御するクラス
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PumpType
{
    _bossAttack,
    _attack
}

public class PumpAI : BaseVegetable
{

    //-----------------------------------------
    // public
    //-----------------------------------------

    public PumpType type;

    //-----------------------------------------
    // private
    //-----------------------------------------

    //近い穴の位置を格納する変数
    GameObject nearHole;

    //近い敵の位置を格納する変数
    GameObject nearEnemy;

    //親の位置を取得する変数
    Vector3 parentPos;

    //デストロイするオブジェクトの変数
    BaseVegetable target;

    BaseBossEnemy boss;

    Hole hole;

    //エネミーに当たった
    bool isEnemyCollision = false;

    //穴に当たった
    bool isHoleCollision = false;

    bool isHoleInfection = true;

    //-----------------------------------------
    // 関数
    //-----------------------------------------

    protected override void DoStart()
    {
        if (type == PumpType._attack) IsMove = true;
        parentPos = transform.position;
        SerchTarget();

        if (type == PumpType._attack)
        {
            IsMove = true;
        }
    }

    protected override void DoUpdate()
    {
        ActionState();


        Death();
    }

    /// <summary>
    /// ターゲットを取得
    /// </summary>
    void SerchTarget()
    {
        //while (isHoleInfection)
        //{
            //親のオブジェクトとHoleタグを探す
            nearHole = serchTarget.serchTag(parentPos, "Hole");

            isHoleInfection = nearHole.GetComponent<Hole>().Infection;
        //}

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
    /// 攻撃処理
    /// </summary>
    protected override void Attack()
    {
        if(IsAttackInterval() == false)
        {
            AddDamage(NearTarget);
        }
    }


    protected override void Death()
    {
        if (IsDestroyEnemy()) Destroy(transform.parent.gameObject);
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
    /// 感染させる
    /// </summary>
    void HoleInfection()
    {
        if (hole != null)
        {
            hole.Infectious();

            //畑が感染したらパンプ菌も消える
            if (hole.Infection && hole.gameObject.tag == "Hole")
                Destroy(transform.parent.gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        boss = col.GetComponent<BaseBossEnemy>();

        if (boss != null)
        {
            isEnemyCollision = true;
            Attack();
        }
        else if (target == null && hole == null)
        {
            //ターゲットにダメージを与える
            target = col.GetComponent<BaseVegetable>();

            //ターゲットにHoleスクリプトがアタッチされていたら
            hole = col.GetComponent<Hole>();

            if (hole != null && hole.Infection == true) hole = null;

            if (target != null || hole != null) IsStop = true;

            if (target != null) isEnemyCollision = true;
            else if (hole != null) isHoleCollision = true;
        }
        
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