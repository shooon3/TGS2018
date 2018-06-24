using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieSprite : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 4.0f);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 vec = transform.position;
        vec.y += 3 * Time.deltaTime;
        transform.position = vec;
	}
}
