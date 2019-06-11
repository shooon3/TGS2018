using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//-----------------------------------------
// 列挙隊
//-----------------------------------------

    /// <summary>
    /// どの行動をするか
    /// </summary>
public enum ActionState
{
    _killVirus = 0,
    _attack,
    _move
}

public abstract class BaseBossEnemy : BaseEnemy {

    //-----------------------------------------
    // public
    //-----------------------------------------

    [Header("パンプキング")]
    public GameObject pumpking;

    public GameObject holeRange;

    public GameObject deadEffect;

    //-----------------------------------------
    // protected
    //-----------------------------------------

    protected ActionState state;

    // 畑を全て格納しておく
    protected List<Hole> holeArray = new List<Hole>();

    // 現在殺菌している畑を格納
    protected List<Hole> hole_NowLis = new List<Hole>();

    //-----------------------------------------
    // private
    //-----------------------------------------

    CameraShake shakeCam;

    // 一番最初のみ処理を行うためのflg
    bool isFirst = true;

    // 感染できる状態かどうか
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
    protected abstract bool IsKillVirus();

    //-----------------------------------------
    // 関数
    //-----------------------------------------

    protected override void DoStart()
    {
        base.DoStart();

        KillVirus_RangeSet();

        // カメラを揺らすためのスクリプト
        shakeCam = Camera.main.GetComponent<CameraShake>();
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        ActionStateSet();
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    protected override void Death()
    {
        // 死んだら
        if (IsDeath())
        {
            // 死んだエフェクトを生成
            Vector3 vec = new Vector3(transform.position.x, transform.position.y + 20.0f, transform.position.z);
            Instantiate(deadEffect, vec, Quaternion.identity);

            // 死んだときの音を再生
            AudioManager.Instance.PlaySE("PumpkinDead");

            // 現在のオブジェクトは削除
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 動く目的地を変更する
    /// </summary>
    protected virtual void MovePointChange()
    {
        // 攻撃できる状態で、攻撃対象がパンプキングでなければ
        if (state == ActionState._attack && NearTarget != pumpking)
        {
            // 対象をパンプキングに変更する
            NearTarget = pumpking;
            // 移動フラグをtrueにする
            IsMove = true;
        }
        // 殺菌可能状態で、NearTargetが真ん中の列の畑(hole_NowLis[1]番目に格納されている畑は必ず真ん中の列の畑)でなければ
        else if (state == ActionState._killVirus && NearTarget != hole_NowLis[1].gameObject)
        {
            // 真ん中の列の畑を目的地として保存する
            NearTarget = hole_NowLis[1].gameObject;
            // 攻撃状態を解除
            isAttack = false;
            // 移動させる
            IsMove = true;
        }
    }

    /// <summary>
    /// ボスの挙動を制御するステートを変更
    /// </summary>
    void ActionStateSet()
    {
        // 殺菌
        if (state == ActionState._killVirus)
        {
            // 殺菌できる状態であれば、殺菌処理を行う
            if (isCanKillVirus) KillBacteria();

            // 全て殺菌出来ていたら、パンプキングを攻撃する
            if (IsKillVirus())
            {
                state = ActionState._attack;
                // 殺菌できる畑は存在しない(全て殺菌した)ためfalse
                isCanKillVirus = false;
            }
        }
        // 攻撃
        else if (state == ActionState._attack)
        {
            // 新たに感染された畑があれば、その畑を殺菌しにいく
            if (IsKillVirus() == false) state = ActionState._killVirus;
            // すべて殺菌された状態であれば、パンプキングを攻撃する
            else if (isAttack) Attack();
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
    /// <returns>全て殺菌できていたらtrue,出来ていないものがあればfalse</returns>
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
        //一番最初のみ演出
        if (isFirst)
        {
            isFirst = false;

            // ボス出現時にカメラを揺らす
            shakeCam.DoShake(2.0f, 1.0f);

            // 音
            AudioManager.Instance.PlaySE("BossPOP");

            // 出現アニメーションが終わるまで待機
            StartCoroutine(WaitAnimEnd("pop", time));
        }
    }

    void OnTriggerEnter(Collider col)
    {
        // 攻撃時だったら
        if (state == ActionState._attack)
        {
            // 当たったものがBossPumpkingでなければ処理をしない
            BossPumpKing target = col.transform.GetComponent<BossPumpKing>();
            if (target == null) return;

            // 目的地に着いたら
            if (agent.isStopped == false)
            {
                // 動きを止める
                IsStop = true;
                // 攻撃する
                isAttack = true;
            }
        }
        // 殺菌時だったら
        else if(state == ActionState._killVirus)
        {
            // あたったのが目標地点(真ん中の畑)だったら
            Hole hole = col.transform.GetComponent<Hole>();
            if (hole == null || hole.gameObject != NearTarget) return;

            // 目的地に着いたら
            if (agent.isStopped == false)
            {
                // 動きを止める
                IsStop = true;
                // 殺菌する
                isCanKillVirus = true;
            }
        }
    }

    /// <summary>
    /// アニメーションを設定
    /// </summary>
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
