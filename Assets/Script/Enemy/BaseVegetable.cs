/*
 * BaseVegetableクラス
 * AIの基底クラス
 * 
 * すべてのAIはこのクラスを基底クラスとすること
 * Start、Updateは抽象メソッドであるDoStart,DoUpdateで実装する
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AnimationType
{
    _move,
    _attack,
    _killVirus,
    _idle
}

public abstract class BaseVegetable : MonoBehaviour {

    //-----------------------------------------
    // private
    //-----------------------------------------

    float animWaitTimer;

    //-----------------------------------------
    // public
    //-----------------------------------------

    public VegetableStatus status;

    public int hp;

    [System.NonSerialized]
    public AnimationType animType;

    //-----------------------------------------
    // protected
    //-----------------------------------------

    protected NavMeshAgent agent;

    protected SerchNearObj serchTarget;

    protected float attackInterval; //攻撃間隔

    protected float intervalTimer; //タイマー

    protected Animator animator;

    protected bool isAnimFirst = true;


    //-----------------------------------------
    // プロパティ
    //-----------------------------------------

    /// <summary>
    /// 一番近いターゲット
    /// </summary>
    public GameObject NearTarget { get; set; }

    /// <summary>
    /// 体力
    /// </summary>
    public int HP { get; protected set; }

    /// <summary>
    /// 攻撃力
    /// </summary>
    public int POW { get; protected set; }

    /// <summary>
    /// 移動できるかどうか
    /// </summary>
    public bool IsMove { get; set; }


    public bool IsStop { get; set; }

    //-----------------------------------------
    // 抽象メソッド
    //-----------------------------------------

    protected abstract void DoStart();

    protected abstract void DoUpdate();

    protected abstract void Death();

    protected abstract void Attack();

    //-----------------------------------------
    // 関数
    //-----------------------------------------

    public void Start()
    {
        SetValue();

        hp = HP;

        DoStart();

        isAnimFirst = true;
        //IsStop = true;
    }

    public void Update()
    {
        Move();
        Stop();

        hp = HP;

        DoUpdate();

        SetAnimaton();

        AttackRotation();

        if (NearTarget != null && agent != null && agent.enabled)
        {
            Transform targetTransform = NearTarget.transform;
            agent.SetDestination(targetTransform.position);
        }

    }

    /// <summary>
    /// 値を初期化するメソッド
    /// </summary>
    void SetValue()
    {
        //コンポーネントを取得
        agent = GetComponent<NavMeshAgent>();
        serchTarget = GetComponent<SerchNearObj>();

        //インターバルを取得
        attackInterval = status.attackInterval;

        //インターバル値を初期化
        intervalTimer = attackInterval;

        //HP,POW値をセット
        HP = status.hp;
        POW = status.pow;

        //移動できる
        IsMove = false;
        IsStop = false;
    }

    /// <summary>
    /// 移動処理
    /// ※）移動する場所が変わるたびに１度呼ぶこと
    /// </summary>
    void Move()
    {
        if (IsMove == false || NearTarget == null || agent == null || agent.enabled == false) return;

        if (agent.isStopped == true) agent.isStopped = false;

        if (agent != null && agent.enabled)
        {

            //agent.SetDestination(targetTransform.position);
            IsMove = false;
        }
    }

    /// <summary>
    /// 移動停止処理
    /// </summary>
    void Stop()
    {
        if (IsStop == false || agent == null || agent.enabled == false) return;

        agent.isStopped = true;

        IsStop = false;
    }

    /// <summary>
    /// 倒されたかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsDeath()
    {
        if (HP <= 0) return true;
        return false;
    }

    /// <summary>
    /// 相手にダメージを与える
    /// </summary>
    public void AddDamage(GameObject target)
    {
        if (target == null) return;

        BaseVegetable targetVegetable = target.GetComponent<BaseVegetable>();

        if (targetVegetable == null) return;

        int targetHP = targetVegetable.HP;

        targetHP -= POW;

        target.GetComponent<BaseVegetable>().HP = targetHP;
    }

    /// <summary>
    ///　攻撃インターバル中かどうか
    /// </summary>
    /// <returns></returns>
    protected bool IsAttackInterval()
    {
        intervalTimer -= Time.deltaTime;

        if (intervalTimer <= 0.0f)
        {
            intervalTimer = attackInterval;
            return false;
        }

        return true;
    }

    /// <summary>
    /// アニメーションをセット
    /// </summary>
    protected virtual void SetAnimaton()
    {
        if (animator == null) return;

        switch (animType)
        {
            case AnimationType._move:
                animator.SetTrigger("IsMove");
                break;

            case AnimationType._attack:
                animator.SetTrigger("IsAttack");
                break;
        }

        AnimDealy();

    }

    void AnimDealy()
    {
        animWaitTimer += Time.deltaTime;

        if (animWaitTimer >= 0.3f)
        {
            if (agent != null && agent.enabled && agent.isStopped && isAnimFirst == false)
            {

                animType = AnimationType._attack;

            }
            else
            {
                animType = AnimationType._move;
            }

            animWaitTimer = 0;
        }
    }

    /// <summary>
    /// 攻撃対象のほうを向く
    /// </summary>
    void AttackRotation()
    {
        if (NearTarget == null && IsStop == false) return;

        if (NearTarget == null) return;

        Vector3 targetRotate = NearTarget.transform.position - transform.position;

        targetRotate = new Vector3(targetRotate.x, 0, targetRotate.z);

        Quaternion rotation = Quaternion.LookRotation(targetRotate);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);
    }
}