using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossPotato : BaseBossEnemy {

    IEnumerable<Hole> holeLis = new List<Hole>();

    bool isRightHole = false;
    bool isFirstDegerming = false;

    protected override void DoStart()
    {
        base.DoStart();

        //IsMove = true;
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        ActionState();
    }

    void ActionState()
    {
        if (HPPercent >= 60)
        {
            AttackStateSet();
            isFirstDegerming = true;
        }
        else if (59 <= HPPercent && IsAllDegerming(isRightHole) == false)
        {
            InfectionStateSet();
        }
        else if(IsAllDegerming(isRightHole))
        {
            AttackStateSet();
            isFirstDegerming = true;
        }
        else if(29 <= HPPercent && IsAllDegerming(isRightHole) == false)
        {
            InfectionStateSet();
        }
        else if(IsAllDegerming(isRightHole))
        {
            AttackStateSet();
        }
    }

    void AttackStateSet()
    {
        int rand = Random.Range(0, 10);
        int childNum = childDestroyObj.transform.childCount;

        if (rand == 0 && childNum > 1) state = BossActionState._shakeAttack;
    }

    void InfectionStateSet()
    {
        if (IsAllDegerming(isRightHole) && isFirstDegerming)
        {
            isFirstDegerming = false;
            isRightHole = true;
            state = BossActionState._move;
        }
        else
        {
            state = BossActionState._infection;
        }
    }

    /// <summary>
    /// すべて除菌できたかどうか
    /// </summary>
    /// <param name="isRight"></param>
    /// <returns></returns>
    bool IsAllDegerming(bool isRight)
    {
        if (isRight) holeLis = holeRightLis;
        else holeLis = holeLeftLis;

        foreach (Hole hole in holeLis)
        {
            if (hole.Infection != false) return false;
        }

        return true;
    }

    protected override void NormalAttack()
    {
        if (isAttack) Attack();
    }

    protected override void ShakeAttack()
    {
        int childNum = childDestroyObj.transform.childCount;

        if (childNum <= 30) return;

        for(int i = 1; i < childNum; i++)
        {
            Destroy(childDestroyObj.transform.GetChild(i).gameObject);
        }
    }

    protected override void Infection()
    {
        foreach (Hole hole in holeLis)
        {
            hole.Decontamination();
        }

    }
}
