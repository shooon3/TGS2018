using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum BossActionState
{
    _move = 0,
    _shakeAttack,
    _bossAttack,
    _infection,
}

public abstract class BaseBossEnemy : BaseEnemy {

    [Header("存在する畑")]
    public GameObject holeRange;

    public Hole[] holeArray;

    public GameObject childDestroyObj;

    public GameObject pumpking;

    protected IEnumerable<Hole> holeRightLis = new List<Hole>();
    protected IEnumerable<Hole> holeLeftLis = new List<Hole>();

    protected BossActionState state;

    /// <summary>
    /// 現在のHPの割合
    /// </summary>
    protected float HPPercent { get; private set; }

    CameraShake shakeCam;

    bool isFirst = true;

    float maxHP;


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

        NearTarget = pumpking;

        maxHP = HP;
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        StartShake(2.5f);

        //SerchTarget();

        AttackState();

        HPPercent = (float)HP / maxHP * 100;
    }

    void AttackState()
    {
        switch(state)
        {
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
    protected void StartShake(float time = 0)
    {
        if (isFirst)
        {
            shakeCam.DoShake(2.0f, 2.0f);
            isFirst = false;

            StartCoroutine(WaitAnimEnd("pop", time));

        }
    }

    void OnTriggerEnter(Collider col)
    {

        BossPumpKing target = col.transform.GetComponent<BossPumpKing>();

        if (target == null) return;

        if (agent.isStopped == false)
        {
            IsStop = true;
            isAttack = true;
        }
    }
}
