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
    BaseVegetable target;

    Hole hole;

    float timer; //攻撃間隔を確認するための変数

    bool isEnemyCollision = false; //エネミーに当たった
    bool isHoleCollision = false; //穴に当たった

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
        if(IsDestroyEnemy())
        {
            Destroy(gameObject);
        }

        Debug.Log("ぱんぷ菌" + HP);

        ActionState();
    }

    void ActionState()
    {
        if (isEnemyCollision) Attack();
        if (isHoleCollision) HoleInfection();
    }

    public override void Attack()
    {
        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            AddDamage(NearTarget);
            timer = attackInterver;
        }
    }

    void HoleInfection()
    {
        if (hole != null)
        {
            hole.Infectious();

            if (hole.Infection) Destroy(gameObject);
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

            if (holeDis <= enemyDis) NearTarget = nearHole;
            else if (holeDis > enemyDis) NearTarget = nearEnemy;
        }
        else NearTarget = nearHole;
    }

    void OnTriggerEnter(Collider col)
    {
        //ターゲットにダメージを与える
        target = col.GetComponent<BaseVegetable>();

        //ターゲットにHoleスクリプトがアタッチされていたら
        hole = col.GetComponent<Hole>();

        if (target != null) isEnemyCollision = true;
        else if (hole != null) isHoleCollision = true;
    }

    //void OnTriggerStay(Collider col)
    //{
    //    if (target != null)
    //    {
    //        Debug.Log("しらとり");
    //        NearTarget = target.gameObject;
    //        Attack();
    //    }


    //}

    bool IsDestroyEnemy()
    {
        if (HP <= 0 || NearTarget == null) return true;
        else return false;
    }
}
