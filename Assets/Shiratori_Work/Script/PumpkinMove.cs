using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PumpkinMove : MonoBehaviour {

    NavMeshAgent agent;

    //SerchHolesスクリプトが取得した位置を取得するための変数
    SerchNearObj serchNearObj;

    //近い穴の位置を格納する変数
    GameObject nerHole;

    //親の位置を取得する変数
    Vector3 parentPos;

	// Use this for initialization
	void Start () {

        agent = GetComponent<NavMeshAgent>();

        //SerchHolesスクリプトを取得
        serchNearObj = gameObject.GetComponentInParent<SerchNearObj>();

        //親のオブジェクトの位置を取得
        parentPos = transform.parent.transform.position;

        //親のオブジェクトとHoleタグを探す
        nerHole = serchNearObj.serchTag(parentPos, "Hole");
    }
	
	// Update is called once per frame
	void Update () {
        agent.destination = nerHole.transform.position;
    }
}
