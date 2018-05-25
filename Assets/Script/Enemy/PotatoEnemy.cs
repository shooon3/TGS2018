using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoEnemy : BaseVegetable {

    [Header("攻撃時間間隔")]
    public float attackInterver;

    public VegetableStatus status;

    SerchNearObj serchTarget;

    VirusDestoroy target;

    float timer; //攻撃間隔を確認するための変数

    bool isTargetAlive;

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

        if(timer <= 0.0f)
        {
            //target.AddDamage(NearTarget);
            timer = attackInterver;
        }
    }

    void SerchTarget()
    {
        if (NearTarget != null) return;

        if (isTargetAlive)
        {
            NearTarget = serchTarget.serchTag(transform.position, "Minion");
        }
        else
        {
            NearTarget = serchTarget.serchTag(transform.position, "Hole");
        }
    }

    void OnTriggerEnter(Collider col)
    {
        //ターゲットにダメージを与える
        target = col.GetComponent<VirusDestoroy>();

        if (target != null)
        {
            NearTarget = target.gameObject;
            Attack();
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (IsDestroyEnemy(col))
        {
            Debug.Log("ok");
            Destroy(gameObject);
        }
    }

    bool IsDestroyEnemy(Collider col)
    {
        Hole hole = col.GetComponent<Hole>();
        
        if (isTargetAlive == false && hole != null) return true;
        else return false;
    }
}
