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

    //-----------------------------------------
    // プロパティ
    //-----------------------------------------

    public GameObject NearTarget { get; set; }

    public int HP { get; protected set; }

    public int POW { get; protected set; }

    //-----------------------------------------
    // 抽象クラス
    //-----------------------------------------

    protected abstract void DoStart();
    protected abstract void DoUpdate();

    public abstract void Attack();

    BaseVegetable targetVegetable;

    // Use this for initialization
    public void Start ()
    {
        agent = GetComponent<NavMeshAgent>();

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

        targetVegetable = target.GetComponent<BaseVegetable>();

        if (targetVegetable == null) return;

        int targetHP = targetVegetable.HP;

        targetHP -= POW;

        target.GetComponent<BaseVegetable>().HP = targetHP;
    }
}