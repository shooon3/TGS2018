/*
 * じゃがいも・だいこん(近接攻撃)の敵AIクラス
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : BaseEnemy {

    protected override void DoStart()
    {
        base.DoStart();

        // 登場アニメーションが終わるまでは待機
        StartCoroutine(WaitAnimEnd("pop", 2.5f));
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        // 攻撃するターゲットを探す
        SerchTarget();

        // 攻撃
        if (isAttack) Attack();
    }

}
