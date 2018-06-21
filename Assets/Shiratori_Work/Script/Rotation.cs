using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    [Header("回転速度")]
    public float speed;

	// Use this for initialization
	void Update ()
    {
        transform.Rotate(new Vector3(0, speed, 0) * Time.deltaTime, Space.World);
    }
}