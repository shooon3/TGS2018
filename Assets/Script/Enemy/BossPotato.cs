using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossPotato : BaseBossEnemy {

    protected override void DoStart()
    {
        base.DoStart();

        StartCoroutine(WaitAnimEnd("pop", 2.5f));
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        state = BossActionState._infection;
    }

    protected override void NormalAttack()
    {
        if (isAttack) Attack();
    }

    protected override void ShakeAttack()
    {
        int childNum = childDestroyObj.transform.childCount;

        if (childNum <= 1) return;

        for(int i = 1; i < childNum; i++)
        {
            Destroy(childDestroyObj.transform.GetChild(i));
        }
    }

    protected override void Infection()
    {
        foreach (Hole hole in holeRightLis)
        {
            hole.Decontamination();
        }

    }
}
