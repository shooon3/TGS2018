using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossActionState
{
    _normalAttack = 0,
    _shakeAttack,
    _infection,
}

public class BaseBossEnemy : BaseEnemy {

    [Header("存在する畑")]
    public GameObject holeRange;

    public Hole[] holeArray;

    CameraShake shakeCam;

    bool isFirst = true;

    BossActionState state;

    protected override void DoStart()
    {
        base.DoStart();

        holeArray = holeRange.GetComponentsInChildren<Hole>();

        shakeCam = Camera.main.GetComponent<CameraShake>();
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        StartShake();

        SerchTarget();

        SerchHole();
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

    protected virtual void SerchHole()
    {
        int holeRandNum = Random.Range(0, holeArray.Length);
    }

    void Infection ()
    {

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
