﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoEnemy : BaseEnemy {

    [Header("攻撃時間間隔")]
    public float attackInterver;

    public VegetableStatus status;

    SerchNearObj serchTarget;

    VirusDestoroy target;　//感染したら消す

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
            Debug.Log("ok");
            //target.AddDamage(NearTarget);
            timer = attackInterver;
        }
    }

    void SerchTarget() //ターゲット切り替え
    {
        if (NearTarget != null) return;

        if (isTargetAlive)
        {
            NearTarget = serchTarget.serchTag(transform.position, "Minion");　//Minionが生きているかどうか
        }
        else
        {
            NearTarget = serchTarget.serchTag(transform.position, "Hole"); //一番近くの穴に戻る
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

    void OnTriggerStay(Collider col)　//StageにPlayerがいないときに畑に触れると穴に帰る
    {
        Debug.Log(NearTarget);
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
