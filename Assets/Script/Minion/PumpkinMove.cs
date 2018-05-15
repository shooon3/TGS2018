using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PumpkinMove : MonoBehaviour {

    NavMeshAgent agent;

    //SerchHolesスクリプトが取得した位置を取得するための変数
    Serch serchHoles;

    //近い穴の位置を格納する変数
    GameObject nerHole;

    //親の位置を取得する変数
    Vector3 parentPos;

	// Use this for initialization
	void Start () {

        agent = GetComponent<NavMeshAgent>();

        //SerchHolesスクリプトを取得
        serchHoles = gameObject.GetComponentInParent<Serch>();

        //親のオブジェクトの位置を取得
        parentPos = transform.parent.transform.position;

        //親のオブジェクトとHoleタグを探す
        nerHole = serchHoles.serchTag(parentPos, "EnemyObj");
    }
	
	// Update is called once per frame
	void Update () {
        agent.destination = nerHole.transform.position;
    }
}
