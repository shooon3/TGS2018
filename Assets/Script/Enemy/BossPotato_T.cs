using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//public enum ActionState
//{
//    _attack,
//    _killBacteria
//}

//public enum KillBacteriaHoleType
//{
//    _right = 0,
//    _left,
//    _rightMiddle,
//    _leftMiddle
//}

public class BossPotato_T : EnemyPotato {

    [Header("存在する畑")]
    public GameObject holeRange;

    Hole[] holeArray;

    ActionState state;
    KillVirusHoleType type;

    IEnumerable<Hole> hole_RightLis = new List<Hole>();
    IEnumerable<Hole> hole_MiddleRighLis = new List<Hole>();
    IEnumerable<Hole> hole_MiddleLeftLis = new List<Hole>();
    IEnumerable<Hole> hole_LeftLis = new List<Hole>();

    protected override void DoStart()
    {
        base.DoStart();

        KillBacteriaHoleSet();
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        ActionStateSet();
    }

    /// <summary>
    /// 菌を殺せる範囲を指定
    /// </summary>
    void KillBacteriaHoleSet()
    {
        holeArray = holeRange.GetComponentsInChildren<Hole>();

        hole_RightLis = holeArray.OrderBy(x => x.name).Skip(0).Take(3);
        hole_MiddleRighLis = holeArray.OrderBy(x => x.name).Skip(3).Take(3);
        hole_MiddleLeftLis = holeArray.OrderBy(x => x.name).Skip(6).Take(3);
        hole_LeftLis = holeArray.OrderBy(x => x.name).Skip(9).Take(3);
    }

    void ActionStateSet()
    {
        int random = Random.Range(0, 100);

        if (random == 0 || state == ActionState._killVirus)
        {
            state = ActionState._killVirus;
            if (IsKillBacteria()) state = ActionState._attack;
        }
        else if(random != 0 && state == ActionState._attack)
        {
            if (isAttack) Attack();
        }
    }

    /// <summary>
    /// 殺菌
    /// </summary>
    bool IsKillBacteria()
    {
        switch(type)
        {
            case KillVirusHoleType._left:
                if(IsHoleKillBacteria(hole_LeftLis)) type = KillVirusHoleType._right;
                return IsHoleKillBacteria(hole_LeftLis);

            case KillVirusHoleType._right:
                if (IsHoleKillBacteria(hole_RightLis)) type = KillVirusHoleType._leftMiddle;
                return IsHoleKillBacteria(hole_RightLis);

            case KillVirusHoleType._leftMiddle:
                if (IsHoleKillBacteria(hole_MiddleLeftLis)) type = KillVirusHoleType._rightMiddle;
                return IsHoleKillBacteria(hole_MiddleLeftLis);

            case KillVirusHoleType._rightMiddle:
                if (IsHoleKillBacteria(hole_MiddleRighLis)) type = KillVirusHoleType._left;
                return IsHoleKillBacteria(hole_MiddleRighLis);
        }
        return false;
    }

    bool IsHoleKillBacteria(IEnumerable<Hole> hole_Lis)
    {
        foreach (Hole hole in hole_Lis)
        {
            hole.Decontamination();
        }

        foreach (Hole hole in hole_Lis)
        {
            if (hole.Infection != false) return false;
        }
        return true;
    }

    void EnemyAttack()
    {

    }

    void OnTriggerEnter(Collider col)
    {

    }
}
