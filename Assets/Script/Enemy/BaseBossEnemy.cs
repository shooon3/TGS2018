using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum BossActionState
{
    _normalAttack = 0,
    _shakeAttack,
    _infection,
}

public abstract class BaseBossEnemy : BaseEnemy {

    [Header("存在する畑")]
    public GameObject holeRange;

    public Hole[] holeArray;

    public GameObject childDestroyObj;

    protected IEnumerable<Hole> holeRightLis = new List<Hole>();
    protected IEnumerable<Hole> holeLeftLis = new List<Hole>();

    CameraShake shakeCam;

    bool isFirst = true;

    protected BossActionState state;

    protected abstract void NormalAttack();

    protected abstract void ShakeAttack();

    protected abstract void Infection();

    protected override void DoStart()
    {
        base.DoStart();

        holeArray = holeRange.GetComponentsInChildren<Hole>();

        holeRightLis = holeArray.OrderBy(x => x.name).Take(6);
        holeLeftLis = holeArray.OrderByDescending(x => x.name).Take(6);

        shakeCam = Camera.main.GetComponent<CameraShake>();
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        StartShake();

        SerchTarget();

        AttackState();
    }

    void AttackState()
    {
        switch(state)
        {
            case BossActionState._normalAttack:
                NormalAttack();
                break;

            case BossActionState._shakeAttack:
                ShakeAttack();
                break;

            case BossActionState._infection:
                Infection();
                break;
        }
    }

    /// <summary>
    /// 出現時に画面を揺らす
    /// </summary>
    void StartShake()
    {
        if (isFirst)
        {
            shakeCam.DoShake(2.0f, 2.0f);
            isFirst = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (state != BossActionState._normalAttack) return;

        PumpAI target = col.transform.GetComponent<PumpAI>();

        if (target == null || target.type == PumpType._bossAttack) return;

        if(agent.isStopped == false)
        {
            NearTarget = target.gameObject;

            IsStop = true;
            isAttack = true;
        }
    }
}
