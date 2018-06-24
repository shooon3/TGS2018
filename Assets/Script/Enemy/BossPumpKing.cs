using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossPumpKing : BaseVegetable {

    public Image damageImage;

    CameraShake shakeCam;

    Animator imageAnim;

    int memoryHP;

    protected override void DoStart()
    {
        imageAnim = damageImage.GetComponent<Animator>();

        shakeCam = Camera.main.GetComponent<CameraShake>();

        memoryHP = HP;
    }

    protected override void DoUpdate()
    {
        if (HP <= 0) return;
        if (IsDamage())
        {
            imageAnim.SetBool("IsDamage", true);
            shakeCam.DoShake(0.5f, 0.5f);
        }
        else imageAnim.SetBool("IsDamage", false);
    }

    /// <summary>
    /// ダメージを受けたかどうか
    /// </summary>
    /// <returns></returns>
    bool IsDamage()
    {
        if (memoryHP == hp) return false;
        memoryHP = HP;
        return true;
    }

    protected override void Attack()
    {
        
    }

    protected override void Death()
    {
        if (IsDeath()) Destroy(gameObject);
    }

}
