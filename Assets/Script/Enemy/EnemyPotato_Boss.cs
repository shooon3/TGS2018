using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPotato_Boss : BaseEnemy {

    protected override void DoStart()
    {
        base.DoStart();

        StartCoroutine(WaitAnimEnd("pop", 2.5f));

        BossPumpKing boss = FindObjectOfType<BossPumpKing>();
        if(boss != null) NearTarget = boss.gameObject;
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        if (isAttack) Attack();
    }

    void OnTriggerEnter(Collider col)
    {
        
    }
}
