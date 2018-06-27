using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepperAnimEffect : MonoBehaviour {

    public bool IsAnimAttackStart { get; private set; }

    public bool IsAnimAttackEnd { get; private set; }

    public void AttackStart()
    {
        IsAnimAttackStart = true;
        IsAnimAttackEnd = false;
    }

    public void AttackEnd()
    {
        IsAnimAttackStart = false;
        IsAnimAttackEnd = true;
    }

}
