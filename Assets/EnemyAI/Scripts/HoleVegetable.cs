using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleVegetable : MonoBehaviour {

    Hole hole;

	// Use this for initialization
	void Start () {
        hole = GetComponent<Hole>();
	}
	
	// Update is called once per frame
	void Update () {
        DestroyVegetable();

    }

    void DestroyVegetable()
    {
        if (hole.Infection && transform.childCount != 0)
            Destroy(transform.GetChild(0).gameObject);
    }
}
