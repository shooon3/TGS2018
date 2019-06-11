using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionPotato : BaseEnemy {

    //-----------------------------------------
    // public
    //-----------------------------------------

    public GameObject attackEffect;

    public GameObject deadEffect;

    //-----------------------------------------
    // private
    //-----------------------------------------

    List<Hole> holeLis = new List<Hole>();

    Hole hole;

    Hole nearTargetHole;

    GameObject effect;

    //近い穴の位置を格納する変数
    GameObject nearHole;

    //近い敵の位置を格納する変数
    GameObject nearEnemy;

    float time;

    //-----------------------------------------
    // 関数
    //-----------------------------------------

    protected override void DoStart()
    {
        base.DoStart();

        StartCoroutine(WaitAnimEnd("pop", 2.5f));

        // 全ての畑をholeLisに追加
        Transform holesTransform = transform.parent.parent.GetChild(0);
        holeLis.AddRange(holesTransform.GetComponentsInChildren<Hole>());
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        if(isAnimFirst != true) SerchTarget();

        if (NearTarget == null)
        {
            isAttack = false;
            Destroy(effect);
            animType = AnimationType._move;
            IsMove = true;
        }

        if (agent != null && agent.enabled && agent.isStopped == false &&
            effect != null) Destroy(effect);

        if (isAttack)
        {
            Attack();
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    protected override void Attack()
    {
        if (IsAttackInterval() != false) return;

        // エフェクトを作れていなければ、エフェクトを生成する
        if (effect == null)
        {
            Vector3 offset = new Vector3(transform.position.x + 1.0f, transform.position.y + 3.0f, transform.position.z + 2.0f);
            effect = Instantiate(attackEffect, offset, Quaternion.identity, transform);
        }

        // ダメージを与える
        AddDamage(NearTarget);

    }

    /// <summary>
    /// ターゲットを探す
    /// </summary>
    protected override void SerchTarget()
    {
        // ターゲットがnullでなければ
        if (NearTarget != null)
        {
            nearTargetHole = NearTarget.GetComponent<Hole>();
        }
        
        if (nearTargetHole == null && NearTarget != null) return;

        Transform enemyTransform = transform.parent.parent.GetChild(4);

        nearEnemy = serchTarget.serchChildTag(transform.position, enemyTransform, "Enemy");

        //敵が存在していたら
        if (nearEnemy != null)
        {
            NearTarget = nearEnemy;

            IsMove = true;
        }
        else if (nearEnemy == null && NearTarget == null)
        {
            StartCoroutine(Delay());
            if(effect != null)Destroy(effect);
        }
        
    }

    /// <summary>
    /// 目的地となる畑を設定する
    /// </summary>
    /// <returns></returns>
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);

        int random = Random.Range(0, holeLis.Count);

        NearTarget = holeLis[random].gameObject;

    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    protected override void Death()
    {
        if (IsDeath())
        {
            Vector3 vec = new Vector3(transform.position.x, transform.position.y + 20.0f, transform.position.z);
            Instantiate(deadEffect, vec, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        BaseBossEnemy boss = col.transform.GetComponent<BaseBossEnemy>();

        if (boss != null) return;

        BaseEnemy target = col.transform.GetComponent<BaseEnemy>();

        if (target != null && agent.isStopped == false)
        {
            NearTarget = target.gameObject;

            IsStop = true;
            isAttack = true;
        }

        if (NearTarget == null) return;

        Hole colHole = col.GetComponent<Hole>();
        Hole targetHole = NearTarget.GetComponent<Hole>();

        if(colHole != null && targetHole != null && colHole == targetHole)
        {
            NearTarget = null;
        }
    }
}
