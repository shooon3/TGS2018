using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PotatoEnemy : BaseVegetable
{

    public VegetableStatus status;

    SerchNearObj serchTarget;

    PumpAI target;

    float timer; //攻撃間隔を確認するための変数

    bool isTargetAlive;

    bool isReturnHole = false;

    protected override void DoStart()
    {
        SetValue();
    }

    protected override void DoUpdate()
    {
        isTargetAlive = serchTarget.IsTargetAlive("Minion");

        SerchTarget();
    }

    /// <summary>
    /// 値を初期化するメソッド
    /// </summary>
    void SetValue()
    {
        serchTarget = GetComponent<SerchNearObj>();

        attackInterval = status.attackInterval;

        HP = status.hp;
        POW = status.pow;
    }

    /// <summary>
    /// 攻撃処理を行うメソッド
    /// </summary>
    protected override void Attack()
    {
        if(IsAttack())
        {
            AddDamage(NearTarget);
        }
    }

    /// <summary>
    /// ターゲットを指定する
    /// </summary>
    void SerchTarget()
    {

        if(isTargetAlive && isReturnHole == true)
            NearTarget = serchTarget.serchTag(transform.position, "Minion");

        if (NearTarget != null) return;

        if (isTargetAlive)
        {
            NearTarget = serchTarget.serchTag(transform.position, "Minion");
            isReturnHole = false;
        }
        else if(isTargetAlive == false)
        {
            NearTarget = serchTarget.serchTag(transform.position, "Hole");
            isReturnHole = true;
        }
    }

    void OnTriggerStay(Collider col)
    {
        Transform targetTransform = col.transform;

        if (targetTransform != null)
        {
            //ターゲットにダメージを与える
            target = targetTransform.GetComponent<PumpAI>();

            if (target != null)
            {
                NearTarget = target.gameObject;
                Attack();
            }
        }

        if (IsDestroyEnemy(col))
        {
            Destroy(gameObject);
        }
    }

    bool IsDestroyEnemy(Collider col)
    {
        Hole hole = col.GetComponent<Hole>();

        if (isTargetAlive == false && hole != null) return true;
        else if (HP <= 0) return true;
        else return false;
    }
}