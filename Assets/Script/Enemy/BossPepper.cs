using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossPepper : BaseBossEnemy {

    [Range(20, 50)]
    public int AttackRange = 20;

    public ParticleSystem particle;

    List<Hole> hole_RightLis = new List<Hole>();
    List<Hole> hole_LeftLis = new List<Hole>();

    PepperAnimEffect animEffect;

    bool isPepperAnimFirst = true;
    public enum KillVirusHoleType
    {
        _right = 0,
        _left,
    }

    KillVirusHoleType type;

    protected override void DoStart()
    {
        base.DoStart();

        animEffect = transform.GetChild(0).GetComponent<PepperAnimEffect>();
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        StartShake(2.5f);

        TargetDistance();

        MovePointChange();

        EffectPlay();
    }

    protected override void KillVirus_RangeSet()
    {
        holeArray = holeRange.GetComponentsInChildren<Hole>().ToList<Hole>();

        hole_RightLis = holeArray.OrderBy(x => x.name).Skip(0).Take(6).ToList<Hole>();
        hole_LeftLis = holeArray.OrderBy(x => x.name).Skip(6).Take(6).ToList<Hole>();
    }

    protected override bool IsKillVirus()
    {
        switch (type)
        {
            case KillVirusHoleType._right:
                hole_NowLis = hole_RightLis;
                if (IsHoleKillVirus()) type = KillVirusHoleType._left;
                return IsHoleKillVirus();

            case KillVirusHoleType._left:
                hole_NowLis = hole_LeftLis;
                if (IsHoleKillVirus()) type = KillVirusHoleType._right;
                return IsHoleKillVirus();
        }
        return false;
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

        Hole checkHole = NearTarget.GetComponent<Hole>();

        if (checkHole != null) return;

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
