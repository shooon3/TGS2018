using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallCamShake : MonoBehaviour {

    CameraShake camShake;

	// Use this for initialization
	void Start () {
        camShake = Camera.main.GetComponent<CameraShake>();
        AudioManager.Instance.PlayBGM("titileMovie");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        camShake.DoShake(1.5f, 0.5f);
        AudioManager.Instance.FadeOutBGM(0.5f);
    }
}
