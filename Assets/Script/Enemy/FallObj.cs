using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObj : MonoBehaviour {

    CameraShake shake;

    public bool isFirst = true;

	// Use this for initialization
	void Start () {
        shake = Camera.main.GetComponent<CameraShake>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isFirst == true)
        {
            if (gameObject.activeSelf == true)
            {
                shake.DoShake(2.0f, 1.0f);
                isFirst = false;
            }
        }
    }
}
