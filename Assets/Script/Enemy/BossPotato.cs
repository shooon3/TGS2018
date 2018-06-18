using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPotato : BaseBossEnemy {

    protected override void DoStart()
    {
        base.DoStart();

        StartCoroutine(WaitAnimEnd("pop", 2.5f));
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        if (isAttack) Attack();
    }
}
