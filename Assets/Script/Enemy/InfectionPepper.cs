using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionPepper : BaseEnemy {

    //public GameObject attackEffect;

    public GameObject deadEffect;

    [Range(20, 50)]
    public int AttackRange = 20;

    public ParticleSystem particle;

    List<Hole> holeLis = new List<Hole>();

    Hole hole;

    Hole nearTargetHole;

    //GameObject effect;

    //近い穴の位置を格納する変数
    GameObject nearHole;

    //近い敵の位置を格納する変数
    GameObject nearEnemy;

    float time;

    bool isPepperAnimFirst = true;
    PepperAnimEffect animEffect;

    protected override void DoStart()
    {
        base.DoStart();


        StartCoroutine(WaitAnimEnd("pop", 2.5f));

        Transform holesTransform = transform.parent.parent.GetChild(0);

        animEffect = transform.GetChild(0).GetComponent<PepperAnimEffect>();

        holeLis.AddRange(holesTransform.GetComponentsInChildren<Hole>());
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        if (isAnimFirst != true) SerchTarget();

        if (NearTarget == null)
        {
            isAttack = false;
            //Destroy(effect);
            animType = AnimationType._move;
            IsMove = true;
        }

        //if (agent != null && agent.enabled && agent.isStopped == false &&
        //    effect != null) Destroy(effect);

        EffectPlay();

        if (isAttack)
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        if (IsAttackInterval() != false) return;

        //if (effect == null)
        //{
        //    Vector3 offset = new Vector3(transform.position.x + 1.0f, transform.position.y + 3.0f, transform.position.z + 2.0f);
        //    effect = Instantiate(attackEffect, offset, Quaternion.identity, transform);
        //}

        AddDamage(NearTarget);

    }

    protected override void SerchTarget()
    {
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
            //if (effect != null) Destroy(effect);
        }

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);

        int random = Random.Range(0, holeLis.Count);

        NearTarget = holeLis[random].gameObject;

    }

    protected override void Death()
    {
        if (IsDeath())
        {
            Vector3 vec = new Vector3(transform.position.x, transform.position.y + 20.0f, transform.position.z);
            Instantiate(deadEffect, vec, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void EffectPlay()
    {
        if (animEffect.IsAnimAttackStart && isPepperAnimFirst)
        {
            particle.Play();
            isPepperAnimFirst = false;
        }
        else if (animEffect.IsAnimAttackEnd && isPepperAnimFirst == false)
        {
            particle.Stop();
            isPepperAnimFirst = true;
        }
    }

    /// <summary>
    /// ターゲットとの距離を取得
    /// </summary>
    void TargetDistance()
    {
        if (NearTarget == null) return;

        Vector3 pos = transform.position;
        Vector3 targetPos = NearTarget.transform.position;

        float dis = Vector3.Distance(pos, targetPos);

        if (dis <= AttackRange)
        {
            IsStop = true;
            isAttack = true;
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

        if (colHole != null && targetHole != null && colHole == targetHole)
        {
            NearTarget = null;
        }
    }
}
