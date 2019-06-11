/*
 * EnemyPepperクラス
 * とうがらしのAIクラス
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPepper : BaseEnemy {

    //-----------------------------------------
    // public
    //-----------------------------------------

    [Range(20,50),Header("遠距離攻撃")]
    public int AttackRange = 20;

    [Header("エフェクト")]
    public ParticleSystem particle;

    //-----------------------------------------
    // private
    //-----------------------------------------

    // 一度だけアニメーションを再生
    bool isPepperAnimFirst = true;

    PepperAnimEffect animEffect;


    //-----------------------------------------
    // 関数
    //-----------------------------------------
    protected override void DoStart()
    {
        base.DoStart();

        particle.Stop();

        StartCoroutine(WaitAnimEnd("pop", 2.5f));

        animEffect = transform.GetChild(0).GetComponent<PepperAnimEffect>();
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        EffectPlay();

        SerchTarget();

        TargetDistance();

        if (isAttack) Attack();
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    protected override void Attack()
    {
        if (IsAttackInterval() == false)
        {
            AddDamage(NearTarget);
        }
    }

    /// <summary>
    /// 攻撃時に攻撃エフェクトを出す
    /// </summary>
    void EffectPlay()
    {
        if (animEffect.IsAnimAttackStart && isPepperAnimFirst)
        {
            particle.Play();
            isPepperAnimFirst = false;
        }
        else if (animEffect.IsAnimAttackEnd && isPepperAnimFirst == false)
        {
            particle.Stop();
            isPepperAnimFirst = true;
        }
    }

    /// <summary>
    /// ターゲットとの距離を取得
    /// </summary>
    void TargetDistance()
    {
        if (NearTarget == null) return;

        Vector3 pos = transform.position;
        Vector3 targetPos = NearTarget.transform.position;

        // 自分とターゲットの距離を測る
        float dis = Vector3.Distance(pos, targetPos);


        // 距離が一定距離以内であれば攻撃開始
        if(dis <= AttackRange)
        {
            IsStop = true;
            isAttack = true;
        }
    }
}
