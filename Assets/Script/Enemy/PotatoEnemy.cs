using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PotatoEnemy : BaseVegetable
{
    //public VegetableStatus status;

    PumpAI target;

    float timer; //攻撃間隔を確認するための変数

    bool isTargetAlive;

    bool isReturnHole = false;

    protected override void DoStart()
    {
        IsMove = true;
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    protected override void DoUpdate()
    {
        isTargetAlive = serchTarget.IsTargetAlive("Minion");
        Move();
        SerchTarget();
    }

    /// <summary>
    /// 攻撃処理を行うメソッド
    /// </summary>
    protected override void Attack()
    {
        if (IsAttack())
        {
            animator.SetTrigger("IsAttack");
            AddDamage(NearTarget);
        }
        else
        {
            animator.SetTrigger("IsMove");
        }
    }

    /// <summary>
    /// ターゲットを指定する
    /// </summary>
    protected override void SerchTarget()
    {
        if(isTargetAlive && isReturnHole == true)
            NearTarget = serchTarget.serchTag(transform.position, "Minion");

        if (NearTarget != null) return;

        if (isTargetAlive)
        {
            NearTarget = serchTarget.serchTag(transform.position, "Minion");
            isReturnHole = false;
        }
        else if(isTargetAlive == false)
        {
            NearTarget = serchTarget.serchTag(transform.position, "Hole");
            isReturnHole = true;
        }
    }

    protected override void Death()
    {
        Destroy(gameObject);
    }

    void OnTriggerStay(Collider col)
    {
        Transform targetTransform = col.transform;

        if (targetTransform != null)
        {
            //ターゲットにダメージを与える
            target = targetTransform.GetComponent<PumpAI>();

            if (target != null)
            {
                NearTarget = target.gameObject;
                Attack();
            }
        }

        if (IsDestroyEnemy(col))
        {
            Death();
        }
    }

    bool IsDestroyEnemy(Collider col)
    {
        Hole hole = col.GetComponent<Hole>();

        if (isTargetAlive == false && hole != null) return true;
        else if (HP <= 0) return true;
        else return false;
    }
}