using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusDestoroy : MonoBehaviour {

    Hole hole;

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTrigerEnter(Collider other)
    {
        hole = other.GetComponent<Hole>();

    }

    void OnTriggerStay(Collider other)
    {

        if (hole != null)
        {
            if (isDestroy())
            {
                Destroy(transform.root.gameObject);
            }
        }
    }

    bool isDestroy()
    {
        PumpkinMove destroy = transform.root.GetChild(0).GetComponent<PumpkinMove>();
        if (hole.Infection == true && destroy != null) return true;
        return false;
    }
}
