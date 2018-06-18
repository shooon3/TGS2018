using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(-5.0f, 0, 0, Space.World);
    }
}
