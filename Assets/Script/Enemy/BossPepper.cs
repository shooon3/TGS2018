using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossPepper : BaseBossEnemy {

    //-----------------------------------------
    // 列挙体
    //-----------------------------------------
    enum KillVirusHoleType
    {
        _right = 0,
        _left,
    }

    KillVirusHoleType type;

    //-----------------------------------------
    // public
    //-----------------------------------------

    [Range(20, 50)]
    public int AttackRange = 20;

    public ParticleSystem particle;

    //-----------------------------------------
    // private
    //-----------------------------------------

    List<Hole> hole_RightLis = new List<Hole>();
    List<Hole> hole_LeftLis = new List<Hole>();

    PepperAnimEffect animEffect;

    bool isPepperAnimFirst = true;

    //-----------------------------------------
    // 関数
    //-----------------------------------------

    protected override void DoStart()
    {
        base.DoStart();

        animEffect = transform.GetChild(0).GetComponent<PepperAnimEffect>();
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        StartShake(4.0f);

        TargetDistance();

        MovePointChange();

        EffectPlay();
    }

    /// <summary>
    /// 一気に殺菌させる範囲を決める
    /// </summary>
    protected override void KillVirus_RangeSet()
    {
        holeArray = holeRange.GetComponentsInChildren<Hole>().ToList();

        hole_RightLis = holeArray.OrderBy(x => x.name).Skip(0).Take(6).ToList();
        hole_LeftLis = holeArray.OrderBy(x => x.name).Skip(6).Take(6).ToList();
    }

    /// <summary>
    /// 殺菌するための畑を変更する
    /// </summary>
    /// <returns>殺菌されていたらtrue、されていなければ殺菌を続ける</returns>
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

    /// <summary>
    /// 攻撃時のエフェクトを再生
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
        // ターゲットがいなければ処理しない
        if (NearTarget == null) return;
        Hole checkHole = NearTarget.GetComponent<Hole>();
        if (checkHole != null) return;

        // 自分と相手の距離を取得
        Vector3 pos = transform.position;
        Vector3 targetPos = NearTarget.transform.position;
        float dis = Vector3.Distance(pos, targetPos);

        // 距離が一定いないであれば
        if (dis <= AttackRange)
        {
            // 移動を停止、攻撃
            IsStop = true;
            isAttack = true;
        }
    }
}
