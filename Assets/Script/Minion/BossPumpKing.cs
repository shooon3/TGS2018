using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossPumpKing : BaseVegetable {

    //-----------------------------------------
    // public
    //-----------------------------------------

    public Image damageImage;

    public Image hpBar;

    //-----------------------------------------
    // private
    //-----------------------------------------

    CameraShake shakeCam;

    Animator imageAnim;

    int memoryHP;

    //-----------------------------------------
    // 関数
    //-----------------------------------------

    protected override void DoStart()
    {
        imageAnim = damageImage.GetComponent<Animator>();

        shakeCam = Camera.main.GetComponent<CameraShake>();

        memoryHP = HP;
    }

    protected override void DoUpdate()
    {
        // HPが０であれば処理しない
        if (HP <= 0) return;

        // HPバーの表示
        hpBar.fillAmount = (float)HP / (float)status.hp;

        if (IsDamage())
        {
            // ダメージを受けた際にカメラを揺らし、画面を赤くする
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
        if (memoryHP == HP) return false;
        memoryHP = HP;
        return true;
    }

    protected override void Attack() { }

    /// <summary>
    /// 死んだとき
    /// </summary>
    protected override void Death()
    {
        if (IsDeath()) Destroy(gameObject);
    }

}
