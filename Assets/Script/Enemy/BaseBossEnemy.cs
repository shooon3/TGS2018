using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ActionState
{
    _attack = 0,
    _killVirus,
    _move
}

public enum KillVirusHoleType
{
    _right = 0,
    _left,
    _rightMiddle,
    _leftMiddle
}

public abstract class BaseBossEnemy : BaseEnemy {

    //-----------------------------------------
    // public
    //-----------------------------------------

    [Header("パンプキング")]
    public GameObject pumpking;
    public GameObject holeRange;

    //-----------------------------------------
    // protected
    //-----------------------------------------

    protected ActionState state;
    protected KillVirusHoleType type;

    protected Hole[] holeArray;

    protected List<Hole> hole_NowLis = new List<Hole>();

    //-----------------------------------------
    // private
    //-----------------------------------------

    CameraShake shakeCam;

    bool isFirst = true;

    bool isCanKillVirus = false;

    //-----------------------------------------
    // 抽象メソッド
    //-----------------------------------------
   
   /// <summary>
   /// １度で殺菌できる範囲を決めるメソッド
   /// </summary>
    protected abstract void KillVirus_RangeSet();

    /// <summary>
    /// 殺菌できたかどうかを調べるメソッド
    /// </summary>
    /// <returns></returns>
    protected abstract bool IsKillVirus();

    //-----------------------------------------
    // 関数
    //-----------------------------------------

    protected override void DoStart()
    {
        base.DoStart();

        KillVirus_RangeSet();

        NearTarget = pumpking;

        shakeCam = Camera.main.GetComponent<CameraShake>();
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        ActionStateSet();
    }

    protected virtual void MovePointChange()
    {
        if (state == ActionState._attack && NearTarget != pumpking)
        {
            NearTarget = pumpking;
            IsMove = true;
        }
        else if (state == ActionState._killVirus && NearTarget != hole_NowLis[1].gameObject)
        {
            NearTarget = hole_NowLis[1].gameObject;
            isAttack = false;
            IsMove = true;
        }
    }

    /// <summary>
    /// ボスの挙動
    /// </summary>
    void ActionStateSet()
    {
        int random = Random.Range(0, 100);

        if (random == 0 || state == ActionState._killVirus)
        {
            state = ActionState._killVirus;

            if (isCanKillVirus) KillBacteria();
            if (IsKillVirus())
            {
                state = ActionState._attack;
                isCanKillVirus = false;
            }
        }
        else if (random != 0 && state == ActionState._attack)
        {
            if (isAttack) Attack();
        }
    }

    /// <summary>
    /// 殺菌
    /// </summary>
    /// <param name="hole_Lis"></param>
    protected void KillBacteria()
    {
        foreach (Hole hole in hole_NowLis)
        {
            hole.Decontamination();
        }
    }

    /// <summary>
    /// 殺菌できていたらtrue
    /// </summary>
    /// <param name="hole_Lis"></param>
    /// <returns></returns>
    protected bool IsHoleKillVirus()
    {
        foreach (Hole hole in hole_NowLis)
        {
            if (hole.Infection != false) return false;
        }
        return true;
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
        if (state == ActionState._attack)
        {
            BossPumpKing target = col.transform.GetComponent<BossPumpKing>();

            if (target == null) return;

            if (agent.isStopped == false)
            {
                IsStop = true;
                isAttack = true;
            }
        }
        else if(state == ActionState._killVirus)
        {
            Hole hole = col.transform.GetComponent<Hole>();

            if (hole == null || hole.gameObject != NearTarget) return;

            if(agent.isStopped == false)
            {
                IsStop = true;
                isCanKillVirus = true;
            }
        }
    }

    protected override void SetAnimaton()
    {
        if (animator == null) return;

        switch(animType)
        {
            case AnimationType._move:
                animator.SetTrigger("IsMove");
                break;

            case AnimationType._attack:
                animator.SetTrigger("IsAttack");
                break;

            case AnimationType._killVirus:
                animator.SetTrigger("IsKillVirus");
                break;
        }

        if (isAttack) animType = AnimationType._attack;
        else if (isCanKillVirus) animType = AnimationType._killVirus;
        else animType = AnimationType._move;
    }
}
