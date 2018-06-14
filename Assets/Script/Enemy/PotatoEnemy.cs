using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PotatoEnemy : BaseVegetable
{
    BaseVegetable target;

    bool isTargetAlive;

    bool isReturnHole = false;

    bool isPumpCol = false;

    protected override void DoStart()
    {
        IsMove = true;
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    protected override void DoUpdate()
    {
        isTargetAlive = serchTarget.IsTargetAlive("Minion");
        SerchTarget();

        if (isPumpCol) Attack();
    }

    /// <summary>
    /// 攻撃処理を行うメソッド
    /// </summary>
    void Attack()
    {
        if (IsAttack())
        {
            AddDamage(NearTarget);
        }
    }

    /// <summary>
    /// ターゲットを指定する
    /// </summary>
    void SerchTarget()
    {
        //穴に戻ろうとしたタイミングで、敵が生成された場合、そちらをターゲットに指定
        if (isTargetAlive && isReturnHole == true)
        {
            IsMove = true;
            NearTarget = serchTarget.serchTag(transform.position, "Minion");
        }

        if (NearTarget != null) return;


        IsMove = true;

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

    void OnTriggerEnter(Collider col)
    {
        target = col.transform.GetComponent<BaseVegetable>();

        if (target != null && agent.isStopped == false)
        {
            NearTarget = target.gameObject;

            IsStop = true;
            isPumpCol = true;

        }
    }

    void OnTriggerStay(Collider col)
    {
        //Transform targetTransform = col.transform;

        //if (targetTransform != null)
        //{
        //    //ターゲットにダメージを与える
        //    target = targetTransform.GetComponent<PumpAI>();

        //    if (target != null)
        //    {
        //        IsStop = true;
        //        NearTarget = target.gameObject;
        //        Attack();
        //    }
        //}

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