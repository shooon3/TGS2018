using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPepper_Boss : BaseEnemy {

    [Range(20, 50)]
    public int AttackRange = 20;

    public ParticleSystem particle;

    bool isPepperAnimFirst = true;
    PepperAnimEffect animEffect;

    BossPumpKing boss;

    protected override void DoStart()
    {
        base.DoStart();

        StartCoroutine(WaitAnimEnd("pop", 2.5f));

        boss = FindObjectOfType<BossPumpKing>();

        animEffect = transform.GetChild(0).GetComponent<PepperAnimEffect>();

        if (boss != null) NearTarget = boss.gameObject;
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        EffectPlay();

        TargetDistance();

        if (NearTarget == null)
        {
            NearTarget = boss.gameObject;
            IsMove = true;
            isAttack = false;
        }

        if (isAttack) Attack();
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

        float dis = Vector3.Distance(pos, targetPos);

        if (dis <= AttackRange)
        {
            IsStop = true;
            isAttack = true;
        }
    }
}
