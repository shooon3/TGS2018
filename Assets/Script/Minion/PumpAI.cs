/*
 * PumpAIクラス
 * 味方AIを制御するクラス
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PumpType
{
    _bossAttack,
    _attack
}

public class PumpAI : BaseVegetable
{

    //-----------------------------------------
    // public
    //-----------------------------------------

    // ボスに攻撃しているか、それ以外か
    public PumpType type;

    // エフェクト
    public GameObject attackEffect;
    public GameObject infectionEffect;
    public GameObject deadEffect;

    //-----------------------------------------
    // private
    //-----------------------------------------

    //近い穴の位置を格納する変数
    GameObject nearHole;

    //近い敵の位置を格納する変数
    GameObject nearEnemy;

    GameObject effect;

    //親の位置を取得する変数
    Vector3 parentPos;

    //デストロイするオブジェクトの変数
    BaseVegetable target;

    // 穴
    Hole hole;

    // カメラ
    Camera cam;

    //エネミーに当たった
    bool isEnemyCollision = false;

    //穴に当たった
    bool isHoleCollision = false;

    bool isCreateEffect = true;

    //-----------------------------------------
    // 関数
    //-----------------------------------------

    protected override void DoStart()
    {
        parentPos = transform.position;

        animator = transform.GetChild(0).GetComponent<Animator>();

        effect = attackEffect;

        if (type == PumpType._attack)
        {
            IsMove = true;
            SerchTarget();
            transform.position = new Vector3(transform.position.x, -9.5f, transform.position.z);
        }
        else
        {
            NearTarget = serchTarget.serchTag(parentPos, "Boss");

            float distance = Vector3.Distance(transform.position, NearTarget.transform.position);
            if (distance >= 7.5f) transform.position = NearTarget.transform.position; 
        }
    }

    protected override void DoUpdate()
    {
        ActionState();

        Death();

        NearTargetDis();

        Debug.Log(animType);

        if (type == PumpType._attack)
        {
            if (NearTarget != null && NearTarget.tag != "Hole" && NearTarget.tag != "Enemy")
            {
                SerchTarget();
            }
        }
        else if (type == PumpType._bossAttack)
        {
            if (NearTarget != null)
            {
                Attack();
            }
        }
    }

    /// <summary>
    /// ターゲットとの距離を測る
    /// </summary>
    void NearTargetDis()
    {
        // ターゲットがいなければ処理をしない
        if (NearTarget == null) return;

        // 距離の算出
        float dis = Vector3.Distance(transform.position, NearTarget.transform.position);

        // 一定距離以内であれば
        if(dis >= 10 && agent != null && agent.enabled && agent.isStopped)
        {
            // 移動を止める
            agent.isStopped = false;
        }
    }

    /// <summary>
    /// ターゲットを取得
    /// </summary>
    void SerchTarget()
    {
        // キャッシュ
        Transform holeTransform = transform.parent.parent.parent.GetChild(0);
        Transform enemyTransform = transform.parent.parent.parent.GetChild(4);

        //親のオブジェクトとHoleタグを探す
        nearHole = serchTarget.serchChildTag(parentPos, holeTransform, "Hole",true);
        
        // 近くの敵を探す
        nearEnemy = serchTarget.serchChildTag(parentPos,enemyTransform, "Enemy");

        //敵が存在していなかったら
        if (nearHole != null && nearEnemy == null)
        {
            //近い穴の距離を取得
            Vector3 nearHolePos = nearHole.transform.position;

            NearTarget = nearHole;
        }
        //敵が存在していたら
        if (nearHole != null && nearEnemy != null)
        {
            //近い穴の距離を取得
            Vector3 nearHolePos = nearHole.transform.position;

            //敵の位置を取得
            Vector3 nearEnemyPos = nearEnemy.transform.position;

            //穴と自分の距離を測る
            float holeDis = Vector3.Distance(transform.position, nearHolePos);

            //敵と自分の距離をはかる
            float enemyDis = Vector3.Distance(transform.position, nearEnemyPos);

            //二つを比べて近いほうをNearTarget(攻撃対象)とする
            if (holeDis <= enemyDis) NearTarget = nearHole;
            else if (holeDis > enemyDis) NearTarget = nearEnemy;
        }
        // 両方ともnullであれば、パンプ菌を消す
        else if (nearEnemy == null && nearHole == null) Destroy(transform.parent.gameObject);
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    protected override void Attack()
    {
        if(IsAttackInterval() == false)
        {
            AddDamage(NearTarget);
        }
    }

    /// <summary>
    /// 感染後・戦闘後のパンプ菌を消す処理
    /// </summary>
    protected override void Death()
    {
        // ターゲットとなる敵を倒すことが出来たら
        if (IsDestroyEnemy())
        {
            // 今までいた位置に、感染した野菜を出現させる
            Vector3 vec = new Vector3(transform.position.x, transform.position.y + 20.0f, transform.position.z);
            Instantiate(deadEffect, vec, Quaternion.identity);

            // パンプ菌が死んだ音を出しておく
            AudioManager.Instance.PlaySE("PumpkinDead");

            // パンプ菌を消す
            Destroy(transform.parent.gameObject);
        }
        // 感染が終わったら
        else if(IsInfection())
        {
            // パンプ菌を消す
            Destroy(transform.parent.gameObject);
        }
    }

    /// <summary>
    /// 感染と敵の攻撃処理を分ける
    /// </summary>
    void ActionState()
    {
        // 敵にも、畑にもあたっていなかったら処理をしない
        if (isEnemyCollision == false && isHoleCollision == false) return;

        EffectSet();

        // 敵と当たっていたら攻撃
        if (isEnemyCollision) Attack();
        // 畑と当たっていたら感染
        if (isHoleCollision) HoleInfection();
        
    }

    /// <summary>
    /// 感染させる
    /// </summary>
    void HoleInfection()
    {
        if (hole != null)
        {
            hole.Infectious();

            //畑が感染したらパンプ菌も消える
            if (hole.Infection && hole.gameObject.tag == "Hole")
                Destroy(transform.parent.gameObject);
        }
    }

    /// <summary>
    /// エフェクトを管理
    /// </summary>
    void EffectSet()
    {
        // 敵に当たっている状態でエフェクトが作られていなければ
        if (isEnemyCollision && isCreateEffect)
        {
            // offsetの位置にエフェクトを生成する
            Vector3 offset = new Vector3(transform.position.x + 1.0f, transform.position.y + 3.0f, transform.position.z + 2.0f);
            effect = Instantiate(attackEffect, offset, Quaternion.identity,transform);
            isCreateEffect = false;

        }
        // 穴に当たっている状態でエフェクトが作られていなければ
        else if(isHoleCollision && isCreateEffect)
        {
            Vector3 offset = new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z);
            effect = Instantiate(infectionEffect, offset, Quaternion.identity, transform);
            isCreateEffect = false;
        }
        // エフェクトが一度でも作られており、感染や敵との戦闘が終わっていたら
        else if(animType == AnimationType._move && isCreateEffect == false)
        {
            // エフェクトを削除
            Destroy(effect);
            isCreateEffect = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (type == PumpType._attack)
        {
            if (target == null && hole == null)
            {
                // 当たったオブジェクトがボス野菜であれば、以下の処理はしない
                BaseBossEnemy bossEnemy = col.GetComponent<BaseBossEnemy>();
                if (bossEnemy != null) return;

                //ターゲットにダメージを与える
                target = col.GetComponent<BaseVegetable>();

                //ターゲットにHoleスクリプトがアタッチされており、感染が終わっていたら
                hole = col.GetComponent<Hole>();
                if (hole != null && hole.Infection == true) hole = null;

                // ターゲットがnullでなければ、移動を停止
                if (target != null || hole != null) IsStop = true;

                // ターゲットがいる場合
                if (target != null)
                {
                    // 当たったやつを攻撃
                    isEnemyCollision = true;
                    NearTarget = target.gameObject;
                }
                // 畑がnullでなければ
                else if (hole != null && hole.tag == "Hole")
                {
                    // 当たった畑を感染させる
                    isHoleCollision = true;
                    NearTarget = hole.gameObject;
                }
            }
        }
    }

    /// <summary>
    /// Enemyが死ぬかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsDestroyEnemy()
    {
        if (HP <= 0) return true;
        else return false;
    }

    /// <summary>
    /// ターゲットとなる畑が感染できたかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsInfection()
    {
        if (NearTarget == null) return true;

        Hole nearHole = NearTarget.GetComponent<Hole>();
        if (nearHole != null && nearHole.Infection) return true; 

        return false;
    }

    /// <summary>
    /// アニメーションを管理
    /// </summary>
    protected override void SetAnimaton()
    {
        if (animator == null) return;

        switch (animType)
        {
            case AnimationType._move:
                animator.SetTrigger("IsMove");
                break;

            case AnimationType._attack:
                animator.SetTrigger("IsAttack");
                break;
        }

        if (type == PumpType._attack && agent != null && agent.enabled && agent.isStopped == true) animType = AnimationType._attack;
        else animType = AnimationType._move;
    }
}