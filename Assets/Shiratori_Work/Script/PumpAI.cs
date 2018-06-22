/*
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

    public GameObject attackEffect;

    //-----------------------------------------
    // private
    //-----------------------------------------

    //近い穴の位置を格納する変数
    GameObject nearHole;

    //近い敵の位置を格納する変数
    GameObject nearEnemy;

    GameObject effect;

    //親の位置を取得する変数
    Vector3 parentPos;

    //デストロイするオブジェクトの変数
    BaseVegetable target;

    Hole hole;

    //エネミーに当たった
    bool isEnemyCollision = false;

    //穴に当たった
    bool isHoleCollision = false;

    bool isHoleInfection = true;

    bool isCreateEffect = true;

    //-----------------------------------------
    // 関数
    //-----------------------------------------

    protected override void DoStart()
    {
        parentPos = transform.position;

        animator = transform.GetChild(0).GetComponent<Animator>();

        effect = attackEffect;

        if (type == PumpType._attack)
        {
            IsMove = true;
            SerchTarget();
        }
        else
        {
            NearTarget = serchTarget.serchTag(parentPos, "Boss");
        }
    }

    protected override void DoUpdate()
    {
        ActionState();

        Death();

        EffectSet();
    }

    /// <summary>
    /// ターゲットを取得
    /// </summary>
    void SerchTarget()
    {
        //親のオブジェクトとHoleタグを探す
        nearHole = serchTarget.serchTag(parentPos, "Hole");

        isHoleInfection = nearHole.GetComponent<Hole>().Infection;

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
        if (isEnemyCollision)
        {
            Attack();
            //GameObject parentObj = NearTarget.transform.FindGameObjectWithTag("parentObj");
        }
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

    void EffectSet()
    {

        if (animType == AnimationType._attack && isCreateEffect)
        {
            Vector3 offset = new Vector3(transform.position.x + 1.0f, transform.position.y + 3.0f, transform.position.z + 2.0f);
            effect = Instantiate(attackEffect, offset, Quaternion.identity,transform);
            isCreateEffect = false;

        }
        else if(animType == AnimationType._move && isCreateEffect == false)
        {
            Destroy(effect);
            isCreateEffect = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (type == PumpType._bossAttack)
        {
            SetParent boss = col.GetComponent<SetParent>();

            if (boss != null)
            {
                isEnemyCollision = true;
                Attack();
            }
        }
        else if (target == null && hole == null)
        {
            BaseBossEnemy bossEnemy = col.GetComponent<BaseBossEnemy>();

            if (bossEnemy != null) return; 

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

    protected override void SetAnimaton()
    {
        if (animator == null) return;

        switch (animType)
        {
            case AnimationType._move:
                animator.SetTrigger("IsMove");
                break;

            case AnimationType._attack:
                animator.SetTrigger("IsAttack");
                break;
        }

        if (agent != null && agent.enabled && agent.isStopped) animType = AnimationType._attack;
        else animType = AnimationType._move;
    }
}