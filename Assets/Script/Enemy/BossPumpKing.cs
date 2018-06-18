using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossPumpKing : BaseVegetable {

    protected override void DoStart()
    {

    }

    protected override void DoUpdate()
    {
        if (hp <= 0) SceneManager.LoadScene("GameOver");
    }

    protected override void Attack()
    {
        
    }

    protected override void Death()
    {
        if (IsDeath()) Destroy(gameObject);
    }

}
