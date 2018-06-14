using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyT : BaseVegetable {

    FallObj fallObj;

	protected override void DoStart()
    {
        IsMove = false;

        fallObj = GetComponent<FallObj>();
    }

    protected override void DoUpdate()
    {
        if(fallObj.isFallStop == false)
        transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime, Space.World);

        Death();
    }

    protected override void Death()
    {
        if (IsDeath()) Destroy(gameObject);
    }
}
