using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossPotato : BaseBossEnemy {

    public enum KillVirusHoleType
    {
        _right = 0,
        _left,
        _rightMiddle,
        _leftMiddle
    }

    KillVirusHoleType type;

    List<Hole> hole_RightLis = new List<Hole>();
    List<Hole> hole_MiddleRighLis = new List<Hole>();
    List<Hole> hole_MiddleLeftLis = new List<Hole>();
    List<Hole> hole_LeftLis = new List<Hole>();

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

        hole_RightLis = holeArray.OrderBy(x => x.name).Skip(0).Take(3).ToList<Hole>();
        hole_MiddleRighLis = holeArray.OrderBy(x => x.name).Skip(3).Take(3).ToList<Hole>();
        hole_MiddleLeftLis = holeArray.OrderBy(x => x.name).Skip(6).Take(3).ToList<Hole>();
        hole_LeftLis = holeArray.OrderBy(x => x.name).Skip(9).Take(3).ToList<Hole>();
    }

    protected override bool IsKillVirus()
    {
        switch (type)
        {
            case KillVirusHoleType._left:
                hole_NowLis = hole_LeftLis;
                if (IsHoleKillVirus()) type = KillVirusHoleType._right;
                return IsHoleKillVirus();

            case KillVirusHoleType._right:
                hole_NowLis = hole_RightLis;
                if (IsHoleKillVirus()) type = KillVirusHoleType._leftMiddle;
                return IsHoleKillVirus();

            case KillVirusHoleType._leftMiddle:
                hole_NowLis = hole_MiddleLeftLis;
                if (IsHoleKillVirus()) type = KillVirusHoleType._rightMiddle;
                return IsHoleKillVirus();

            case KillVirusHoleType._rightMiddle:
                hole_NowLis = hole_MiddleRighLis;
                if (IsHoleKillVirus()) type = KillVirusHoleType._left;
                return IsHoleKillVirus();
        }
        return false;
    }

}
