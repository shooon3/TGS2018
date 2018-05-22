using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmInfection : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Infection()
    {

    }

    void OnTriggerStay(Collider col)
    {
        Hole hole = col.GetComponent<Hole>();

        if(hole != null)
        {
            hole.Infectious();
        }
    }
}
