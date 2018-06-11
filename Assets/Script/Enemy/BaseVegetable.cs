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

[RequireComponent(typeof(SerchNearObj))]
public abstract class BaseVegetable : MonoBehaviour {

    //-----------------------------------------
    // private
    //-----------------------------------------

    bool isMove = true;

    //-----------------------------------------
    // public
    //-----------------------------------------

    public VegetableStatus status;

    //-----------------------------------------
    // protected
    //-----------------------------------------

    protected NavMeshAgent agent;

    protected SerchNearObj serchTarget;

    protected float attackInterval; //攻撃間隔

    protected float intervalTimer; //タイマー

    protected Animator animator;

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
    public bool IsMove
    {
        get { return isMove; }
        set { isMove = value; }
    }

    //-----------------------------------------
    // 抽象メソッド
    //-----------------------------------------

    protected abstract void DoStart();

    protected abstract void DoUpdate();

    protected abstract void Attack();

    protected abstract void SerchTarget();

    protected abstract void Death();

    //-----------------------------------------
    // 関数
    //-----------------------------------------

    public void Start()
    {
        SetValue();

        DoStart();
	}

    public void Update()
    {
        //Move();
        DoUpdate();
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
        IsMove = true;
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Move()
    {
        if (IsMove == false || NearTarget == null || agent.enabled == false) return;

        Transform targetTransform = NearTarget.transform;
        agent.SetDestination(targetTransform.position);
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
    /// 攻撃できるかどうか
    /// </summary>
    /// <returns></returns>
    protected bool IsAttack()
    {
        intervalTimer -= Time.deltaTime;

        if (intervalTimer <= 0.0f)
        {
            intervalTimer = attackInterval;
            return true;
        }

        return false;
    }
}