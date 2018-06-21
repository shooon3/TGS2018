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
    // protected
    //-----------------------------------------

    //ターゲットが生きているかどうか
    protected bool isTargetAlive = true;

    //攻撃できる状態かどうか
    protected bool isAttack = false;

    //-----------------------------------------
    // 関数
    //-----------------------------------------

    protected override void DoStart()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    protected override void DoUpdate()
    {
        AttackRotation();
        Death();
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    protected override void Death()
    {
        if (IsDeath()) Destroy(gameObject);
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

    /// <summary>
    /// 攻撃対象のほうを向く
    /// </summary>
    void AttackRotation()
    {
        if (IsStop == false && NearTarget == null) return;

        Vector3 targetRotate = NearTarget.transform.position - transform.position;

        targetRotate = new Vector3(targetRotate.x, 0, targetRotate.z);

        Quaternion rotation = Quaternion.LookRotation(targetRotate);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);
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
