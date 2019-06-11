/*
 * BaseEnemyクラス
 * EnemyAIの基底クラス
 * 
 * すべてのEnemyAIはこのクラスを継承すること
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemy : BaseVegetable {

    //-----------------------------------------
    // public
    //-----------------------------------------

    public GameObject InfectionEnemy;

    //-----------------------------------------
    // protected
    //-----------------------------------------

    //攻撃できる状態かどうか
    protected bool isAttack = false;

    //-----------------------------------------
    // private
    //-----------------------------------------

    // Enemyを管理するオブジェクト
    Transform EnemyTransform;

    //-----------------------------------------
    // 関数
    //-----------------------------------------

    protected override void DoStart()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        EnemyTransform = transform.parent.parent.Find("EnemyParent");
    }

    protected override void DoUpdate()
    {
        // 死んだときの処理
        Death();

        // 近くにターゲットがいなければ、攻撃しない
        if (NearTarget == null) isAttack = false;
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    protected override void Death()
    {
        if (IsDeath())
        {
            // 倒された時、感染した敵を生成する
            Instantiate(InfectionEnemy,transform.position,transform.rotation, EnemyTransform);
            // 現在のオブジェクトは削除
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
        // 設定したオブジェクトがnullでなければ処理をしない
        if (NearTarget != null) return;

        IsMove = true;

        // 近い敵
        NearTarget = serchTarget.serchTag(transform.position, "Minion");
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

        // 最初の登場アニメーションが終わるまで、行動をさせない
        while (isAnimFirst)
        {
            AnimatorStateInfo nowState = animator.GetCurrentAnimatorStateInfo(0);

            // 現在のAnimatorStateがanimNameの引数で渡したものかどうか
            if (nowState.IsName(animName))
            {
                // アニメーションが終わったので、行動を許可
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
             
            // 何かと当たったら、攻撃を開始する
            IsStop = true;
            isAttack = true;
        }
    }
}
