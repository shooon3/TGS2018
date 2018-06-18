using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPumpKing : BaseVegetable {

    protected override void DoStart()
    {

    }

    protected override void DoUpdate()
    {

    }

    protected override void Attack()
    {
        
    }

    protected override void Death()
    {
        if (IsDeath()) Destroy(gameObject);
    }

}
