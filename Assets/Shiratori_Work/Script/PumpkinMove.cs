using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PumpkinMove : MonoBehaviour {

    NavMeshAgent agent;

    NavMeshAgent parentAgent;

    //SerchHolesスクリプトが取得した位置を取得するための変数
    SerchNearObj serchNearObj;

    //移動するポイント
    GameObject moveTarget;

    //親の位置を取得する変数
    GameObject leaderObj;

    Animator animator;

    PumpAI ai;

    //bool isMove = false;

    public PumpType type;

    AnimationType animType;

	// Use this for initialization
	void Start () {

        //親のオブジェクトの位置を取得
        leaderObj = transform.parent.GetChild(0).gameObject;

        animator = transform.GetChild(0).GetComponent<Animator>();

        SerchMovePoint();

        if (ai.type == PumpType._attack)
        {
            agent = GetComponent<NavMeshAgent>();
            parentAgent = leaderObj.GetComponent<NavMeshAgent>();
        }

    }

	// Update is called once per frame
	void Update () {

        if (moveTarget == null && type != PumpType._attack) return;

        if (ai.type == PumpType._attack)
        {
            SetAnimaton();

            agent.SetDestination(moveTarget.transform.position);

            if (parentAgent.isStopped == false) agent.isStopped = false;
            else if (parentAgent.isStopped) agent.isStopped = true;
        }
    }

    void SerchMovePoint()
    {
        ai = leaderObj.GetComponent<PumpAI>();
        moveTarget = ai.NearTarget;
    }

    void SetAnimaton()
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

        if (agent != null && agent.enabled && agent.isStopped) animType = AnimationType._attack;
        else animType = AnimationType._move;
    }
}
