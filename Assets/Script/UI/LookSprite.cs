using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookSprite : MonoBehaviour {

    GameObject front;

    // Use this for initialization
    void Start () {
        front = Camera.main.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
        transform.LookAt(front.transform.position);
	}
}
