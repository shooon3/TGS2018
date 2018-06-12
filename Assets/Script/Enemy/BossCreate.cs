using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCreate : MonoBehaviour {

    public GameObject boss;

    public Transform createPos;

    HoleInfection holeInf;

    bool isFirst = true; //１回だけ生成

	// Use this for initialization
	void Start () {
        boss.SetActive(false);
        //boss.transform.position = new Vector3(createPos.position.x, 30, createPos.position.z);
        holeInf = GetComponent<HoleInfection>();
	}
	
	// Update is called once per frame
	void Update () {
		if(holeInf.AllInfection() && isFirst)
        {
            boss.SetActive(true);
            //Instantiate(boss,new Vector3(createPos.position.x,30,createPos.position.z),boss.transform.rotation);
            isFirst = false;
        }
	}
}
