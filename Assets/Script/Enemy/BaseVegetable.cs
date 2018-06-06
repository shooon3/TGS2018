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
    // protected
    //-----------------------------------------

    protected NavMeshAgent agent;

    protected float attackInterval; //攻撃間隔
    protected float intervalTimer; //タイマー

    //-----------------------------------------
    // プロパティ
    //-----------------------------------------

    public GameObject NearTarget { get; set; }

    public int HP { get; protected set; }

    public int POW { get; protected set; }

    //-----------------------------------------
    // 抽象メソッド
    //-----------------------------------------

    protected abstract void DoStart();

    protected abstract void DoUpdate();

    protected abstract void Attack();

    //-----------------------------------------
    // 関数
    //-----------------------------------------
    
    public void Start ()
    {
        agent = GetComponent<NavMeshAgent>();

        //値を初期化
        intervalTimer = attackInterval;

        DoStart();
	}

    public void Update()
    {
        Move();

        DoUpdate();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        if (NearTarget == null) return;
        Transform targetTransform = NearTarget.transform;

        agent.SetDestination(targetTransform.position);
    }

    /// <summary>
    /// 倒されたかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsDie()
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