/*
 * EnemyPepperクラス
 * とうがらしのAIクラス
 * 
 * ぺっぱーくん
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPepper : BaseEnemy {

    [Range(20,50)]
    public int AttackRange = 20;

    public ParticleSystem particle;

    protected override void DoStart()
    {
        base.DoStart();

        StartCoroutine(WaitAnimEnd("pop", 2.5f));
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

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
            particle.Play();
            AddDamage(NearTarget);
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

        float dis = Vector3.Distance(pos, targetPos);

        if(dis <= AttackRange)
        {
            IsStop = true;
            isAttack = true;
        }
    }
}
