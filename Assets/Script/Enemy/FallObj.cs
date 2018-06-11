using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObj : MonoBehaviour {

    Rigidbody rg;

    CameraShake shake;

    public bool isFallStop = true;

	// Use this for initialization
	void Start () {
        rg = GetComponent<Rigidbody>();
        shake = Camera.main.gameObject.GetComponent<CameraShake>();
	}
	
	// Update is called once per frame
	void Update () {
        if(isFallStop == true) rg.AddForce(Vector3.down * 30.5f);
	}

    void OnCollisionEnter(Collision col)
    {
        if (isFallStop == false) return;
        rg.velocity = Vector3.zero;
        shake.DoShake(0.7f,1.0f);
        isFallStop = false;
    }
}
