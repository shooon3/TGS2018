using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class BossRadish : BaseBossEnemy {

    public enum KillVirusHoleType
    {
        _front = 0,
        _middle,
        _back
    }

    KillVirusHoleType type;

    List<Hole> hole_FrontLis = new List<Hole>();
    List<Hole> hole_Middle = new List<Hole>();
    List<Hole> hole_Back = new List<Hole>();

    protected override void DoStart()
    {
        base.DoStart();
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        StartShake(2.5f);

        MovePointChange();
    }

    protected override void KillVirus_RangeSet()
    {
        holeArray = holeRange.GetComponentsInChildren<Hole>().ToList<Hole>();

        hole_FrontLis = holeArray.GetRange(0, 4);
        hole_Middle = holeArray.GetRange(4, 4);
        hole_Back = holeArray.GetRange(8, 4);
    }

    protected override bool IsKillVirus()
    {
        switch (type)
        {
            case KillVirusHoleType._front:
                hole_NowLis = hole_FrontLis;
                if (IsHoleKillVirus()) type = KillVirusHoleType._middle;
                return IsHoleKillVirus();

            case KillVirusHoleType._middle:
                hole_NowLis = hole_Middle;
                if (IsHoleKillVirus()) type = KillVirusHoleType._back;
                return IsHoleKillVirus();

            case KillVirusHoleType._back:
                hole_NowLis = hole_Back;
                if (IsHoleKillVirus()) type = KillVirusHoleType._front;
                return IsHoleKillVirus();
        }
        return false;
    }

}
