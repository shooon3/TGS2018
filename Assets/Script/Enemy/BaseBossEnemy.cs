using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBossEnemy : BaseVegetable {

    protected override void DoStart()
    {

    }

    protected override void DoUpdate()
    {
        Death();
    }

    protected override void Death()
    {
        if (IsDeath())
        {
            Destroy(gameObject);
        }
    }
}
