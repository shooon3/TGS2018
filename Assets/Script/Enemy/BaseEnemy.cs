/*
 * BaseEnemyクラス
 * EnemyAIの基底クラス
 * 
 * すべてのEnemyAIはこのクラスを継承すること
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseVegetable {

    //-----------------------------------------
    // public
    //-----------------------------------------

    public GameObject InfectionEnemy;

    //-----------------------------------------
    // protected
    //-----------------------------------------

    //ターゲットが生きているかどうか
    protected bool isTargetAlive = true;

    //攻撃できる状態かどうか
    protected bool isAttack = false;

    Transform pumpkinTransform;

    //-----------------------------------------
    // 関数
    //-----------------------------------------

    protected override void DoStart()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        pumpkinTransform = transform.parent.parent.Find("EnemyParent");
    }

    protected override void DoUpdate()
    {
        Death();

        if (NearTarget == null) isAttack = false;
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    protected override void Death()
    {
        if (IsDeath())
        {
            Instantiate(InfectionEnemy,transform.position,transform.rotation, pumpkinTransform);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    protected override void Attack()
    {
        if (IsAttackInterval() == false) AddDamage(NearTarget);
    }

    /// <summary>
    /// ターゲットを取得
    /// </summary>
    protected virtual void SerchTarget()
    {
        if (NearTarget != null) return;

        IsMove = true;

        if (isTargetAlive)
        {
            //NearTarget = serchTarget.serchChildTag(transform.position, pumpkinTransform, "Minion",true);

            NearTarget = serchTarget.serchTag(transform.position, "Minion");
        }
    }

    /// <summary>
    /// アニメーションの終了まで待機する処理
    /// </summary>
    /// <param name="animName">アニメーションの名前</param>
    /// <param name="waitTime">アニメーション終了後の待機時間</param>
    /// <returns></returns>
    protected IEnumerator WaitAnimEnd(string animName, float waitTime = 0)
    {
        if (agent == null || agent.enabled == false) yield break;

        IsStop = true;

        while (isAnimFirst)
        {
            AnimatorStateInfo nowState = animator.GetCurrentAnimatorStateInfo(0);
            if (nowState.IsName(animName))
            {
                yield return new WaitForSeconds(waitTime);
                IsMove = true;
                isAnimFirst = false;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        BaseVegetable target = col.transform.GetComponent<BaseVegetable>();

        if (target != null && agent.isStopped == false)
        {
            NearTarget = target.gameObject;
             
            IsStop = true;
            isAttack = true;
        }
    }
}
