using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BossCreate : MonoBehaviour {

    public GameObject boss;

    Hole[] holes;

    bool isFirst = true; //１回だけ生成

    HoleInfection infection;

    public float InfectPercent { get; set; }

	// Use this for initialization
	void Start () {
        boss.SetActive(false);

        holes = transform.GetComponentsInChildren<Hole>();

        infection = GetComponent<HoleInfection>();
	}
	
	// Update is called once per frame
	void Update () {
       if(infection.InfectionCount() >= 6)
        {
            boss.SetActive(true);
        }
	}
}
