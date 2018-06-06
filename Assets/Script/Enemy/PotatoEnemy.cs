using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoEnemy : BaseVegetable
{

    [Header("攻撃時間間隔")]
    public float attackInterver;

    public VegetableStatus status;

    SerchNearObj serchTarget;

    PumpAI target;

    float timer; //攻撃間隔を確認するための変数

    bool isTargetAlive;

    bool isReturnHole = false;

    //１フレーム前のフラグ状況
    bool isTargetAliveMemory;

    protected override void DoStart()
    {
        serchTarget = GetComponent<SerchNearObj>();
        timer = attackInterver;

        HP = status.hp;
        POW = status.pow;

    }

    protected override void DoUpdate()
    {
        isTargetAlive = serchTarget.IsTargetAlive("Minion");

        SerchTarget();
    }

    public override void Attack()
    {
        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            AddDamage(target.gameObject);
            timer = attackInterver;
        }
    }

    void SerchTarget()
    {

        if(isTargetAlive && isReturnHole == true) NearTarget = serchTarget.serchTag(transform.position, "Minion");

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

    void OnTriggerEnter(Collider col)
    {
        
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