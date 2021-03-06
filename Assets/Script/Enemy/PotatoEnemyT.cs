﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PotatoEnemyT : BaseVegetable
{
    BaseVegetable target;

    bool isTargetAlive;

    bool isReturnHole = false;

    bool isPumpCol = false;

    protected override void DoStart()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        StartCoroutine(WaitAnimEnd("pop",2.5f));
    }

    protected override void DoUpdate()
    {
        isTargetAlive = serchTarget.IsTargetAlive("Minion");
        SerchTarget();
        

        if (isPumpCol) Attack();

        Death();
    }

    /// <summary>
    /// 攻撃処理を行うメソッド
    /// </summary>
    protected override void Attack()
    {
        //if (IsAttack())
        //{
        //    AddDamage(NearTarget);
        //}
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
        if(HP <= 0) Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        GameObject target = col.transform.GetComponent<BaseVegetable>().gameObject;

        if (target != null && agent.isStopped == false)
        {
            NearTarget = target;

            IsStop = true;
            isPumpCol = true;

        }
    }

    IEnumerator WaitAnimEnd(string animName,float waitTime)
    {

        while (!IsMove)
        {
            AnimatorStateInfo nowState = animator.GetCurrentAnimatorStateInfo(0);
            if (nowState.IsName(animName))
            {
                yield return new WaitForSeconds(waitTime);
                IsMove = true;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}