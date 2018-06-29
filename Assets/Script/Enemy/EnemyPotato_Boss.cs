using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPotato_Boss : BaseEnemy {

    BossPumpKing boss;

    protected override void DoStart()
    {
        base.DoStart();

        StartCoroutine(WaitAnimEnd("pop", 2.5f));

        boss = FindObjectOfType<BossPumpKing>();
        if(boss != null) NearTarget = boss.gameObject;
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        if(NearTarget == null)
        {
            NearTarget = boss.gameObject;
            IsMove = true;
            isAttack = false;
        }

        if (isAttack) Attack();
    }

    void OnTriggerEnter(Collider col)
    {
        BaseVegetable target = col.transform.GetComponent<BaseVegetable>();

        if (target != null && agent.isStopped == false)
        {
            NearTarget = target.gameObject;

            IsStop = true;
            isAttack = true;
        }
    }
}
