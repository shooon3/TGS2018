using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpAI : BaseVegetable
{

    [Header("攻撃時間間隔")]
    public float attackInterver;

    public VegetableStatus status;

    //近いターゲットを格納する変数
    SerchNearObj serchTarget;

    //近い穴の位置を格納する変数
    GameObject nearHole;

    //近い敵の位置を格納する変数
    GameObject nearEnemy;

    //移動するポイント
    GameObject moveTarget;

    //親の位置を取得する変数
    Vector3 parentPos;

    //デストロイするオブジェクトの変数
    VirusDestoroy target;

    float timer; //攻撃間隔を確認するための変数

    bool isTargetAlive;

    // Use this for initialization
    protected override void DoStart()
    {

        serchTarget = GetComponent<SerchNearObj>();
        timer = attackInterver;
        //親のオブジェクトの位置を取得
        parentPos = transform.parent.transform.position;

        HP = status.hp;
        POW = status.pow;

        SerchTarget();
    }

    // Update is called once per frame
    protected override void DoUpdate()
    {
        Move();
    }

    void Move()
    {
        if (moveTarget == null) return; 
        agent.destination = moveTarget.transform.position;
    }

    public override void Attack()
    {
        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            Debug.Log("ok");
            //target.AddDamage(NearTarget);
            timer = attackInterver;
        }
    }

    void SerchTarget()
    {
        //親のオブジェクトとHoleタグを探す
        nearHole = serchTarget.serchTag(parentPos, "Hole");

        nearEnemy = serchTarget.serchTag(parentPos, "Enemy");

        Vector3 nearHolePos = nearHole.transform.position;

        if (nearEnemy != null)
        {
            Vector3 nearEnemyPos = nearEnemy.transform.position;

            float holeDis = Vector3.Distance(transform.position, nearHolePos);
            float enemyDis = Vector3.Distance(transform.position, nearEnemyPos);

            if (holeDis <= enemyDis) moveTarget = nearHole;
            else if (holeDis > enemyDis) moveTarget = nearEnemy;
        }
        else moveTarget = nearHole;
    }

    void OnTriggerEnter(Collider col)
    {
        //ターゲットにダメージを与える
        target = col.GetComponent<VirusDestoroy>();

        if (target != null)
        {
            NearTarget = target.gameObject;
            Attack();
        }
    }

    void OnTriggerStay(Collider col)
    {
        Debug.Log(NearTarget);
        if (IsDestroyEnemy(col))
        {
            Debug.Log("ok");
            Destroy(gameObject);
        }
    }

    bool IsDestroyEnemy(Collider col)
    {
        Hole hole = col.GetComponent<Hole>();

        if (isTargetAlive == false && hole != null) return true;
        else return false;
    }
}
