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

    public GameObject infectionEffect;

    public GameObject deadEffect;

    public GameObject near;

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

    Camera cam;

    //エネミーに当たった
    bool isEnemyCollision = false;

    //穴に当たった
    bool isHoleCollision = false;

    //bool isHoleInfection = true;

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
            transform.position = new Vector3(transform.position.x, -9.5f, transform.position.z);
        }
        else
        {
            NearTarget = serchTarget.serchTag(parentPos, "Boss");

            float distance = Vector3.Distance(transform.position, NearTarget.transform.position);
            if (distance >= 7.5f) transform.position = NearTarget.transform.position; 
        }
    }

    protected override void DoUpdate()
    {
        ActionState();

        Death();

        NearTargetDis();

        near = NearTarget;

        if (type == PumpType._attack)
        {
            if (NearTarget != null && NearTarget.tag != "Hole" && NearTarget.tag != "Enemy")
            {
                SerchTarget();
            }
        }
        else if (type == PumpType._bossAttack)
        {
            if (NearTarget != null)
            {
                Attack();
            }
        }
    }

    void NearTargetDis()
    {
        if (NearTarget == null) return;

        float dis = Vector3.Distance(transform.position, NearTarget.transform.position);

        if(dis >= 10 && agent != null && agent.enabled && agent.isStopped)
        {
            agent.isStopped = false;
        }
    }

    /// <summary>
    /// ターゲットを取得
    /// </summary>
    void SerchTarget()
    {
        Transform holeTransform = transform.parent.parent.parent.GetChild(0);
        Transform enemyTransform = transform.parent.parent.parent.GetChild(4);

        //親のオブジェクトとHoleタグを探す
        nearHole = serchTarget.serchChildTag(parentPos, holeTransform, "Hole",true);
        
        nearEnemy = serchTarget.serchChildTag(parentPos,enemyTransform, "Enemy");

        //敵が存在していなかったら
        if (nearHole != null && nearEnemy == null)
        {
            //近い穴の距離を取得
            Vector3 nearHolePos = nearHole.transform.position;

            NearTarget = nearHole;
        }
        //敵が存在していたら
        if (nearHole != null && nearEnemy != null)
        {
            //近い穴の距離を取得
            Vector3 nearHolePos = nearHole.transform.position;

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
        else if (nearEnemy == null && nearHole == null) Destroy(transform.parent.gameObject);
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
        if (IsDestroyEnemy())
        {
            Vector3 vec = new Vector3(transform.position.x, transform.position.y + 20.0f, transform.position.z);
            Instantiate(deadEffect, vec, Quaternion.identity);
            AudioManager.Instance.PlaySE("PumpkinDead");
            Destroy(transform.parent.gameObject);
        }
        else if(IsInfection())
        {
            Destroy(transform.parent.gameObject);
        }
    }

    /// <summary>
    /// 感染と敵の攻撃処理を分ける
    /// </summary>
    void ActionState()
    {
        if (isEnemyCollision == false && isHoleCollision == false) return;

        EffectSet();

        if (isEnemyCollision)
        {
            Attack();
        }
        if (isHoleCollision)
        {
            HoleInfection();
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
            if (hole.Infection && hole.gameObject.tag == "Hole")
                Destroy(transform.parent.gameObject);
        }
    }

    void EffectSet()
    {
        if (isEnemyCollision && isCreateEffect)
        {
            Vector3 offset = new Vector3(transform.position.x + 1.0f, transform.position.y + 3.0f, transform.position.z + 2.0f);
            effect = Instantiate(attackEffect, offset, Quaternion.identity,transform);
            isCreateEffect = false;

        }
        else if(isHoleCollision && isCreateEffect)
        {
            Vector3 offset = new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z);
            effect = Instantiate(infectionEffect, offset, Quaternion.identity, transform);
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
        if (type == PumpType._attack)
        {
            if (target == null && hole == null)
            {
                BaseBossEnemy bossEnemy = col.GetComponent<BaseBossEnemy>();

                if (bossEnemy != null) return;

                //ターゲットにダメージを与える
                target = col.GetComponent<BaseVegetable>();

                //ターゲットにHoleスクリプトがアタッチされていたら
                hole = col.GetComponent<Hole>();

                if (hole != null && hole.Infection == true) hole = null;

                if (target != null || hole != null) IsStop = true;

                if (target != null)
                {
                    isEnemyCollision = true;
                    NearTarget = target.gameObject;
                }
                else if (hole != null && hole.tag == "Hole")
                {
                    isHoleCollision = true;
                    NearTarget = hole.gameObject;
                }
            }
        }
    }

    /// <summary>
    /// Enemyが死ぬかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsDestroyEnemy()
    {
        if (HP <= 0) return true;
        else return false;
    }

    public bool IsInfection()
    {
        if (NearTarget == null) return true;

        Hole nearHole = NearTarget.GetComponent<Hole>();
        if (nearHole != null && nearHole.Infection) return true; 

        return false;
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

        if (type == PumpType._attack && agent != null && agent.enabled && agent.isStopped == true) animType = AnimationType._attack;
        else animType = AnimationType._move;
    }
}