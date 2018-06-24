using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionDestroy : MonoBehaviour {

    public HoleInfection holeInf;

    public GameObject boss;

    CameraShake cam;

    bool isFirst = true;

	// Use this for initialization
	void Start () {
        cam = Camera.main.GetComponent<CameraShake>();
	}
	
	// Update is called once per frame
	void Update () {
		if(holeInf.AllInfection())
        {
            if (isFirst)
            {
                cam.DoShake(1.0f, 0.5f);
                isFirst = false;
            }

            if (boss != null)
            {
                Destroy(boss);
            }
            foreach(Transform t in transform)
            {
                Destroy(t.gameObject);
            }
        }
	}

}
