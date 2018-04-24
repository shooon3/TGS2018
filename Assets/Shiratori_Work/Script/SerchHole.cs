using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerchHole : MonoBehaviour {

    [SerializeField, Header("移動速度")]
    private float speed;
    [SerializeField,Header("回転速度")]
    private float rotationsmooth;
    [SerializeField, Header("穴")]
    private Transform Hole;
    [SerializeField,Header("")]
    private float timeScale;

	// Use this for initialization
	void Start () {
        timeScale = Time.timeScale;

        //穴の位置を発見
        Hole = GameObject.FindWithTag("Hole").transform;

	}

	// Update is called once per frame
	void Update () {
        //穴の方向を向く
        Quaternion targetRotation = Quaternion.LookRotation(Hole.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationsmooth * timeScale);

        //前方に進む
        transform.Translate(Vector3.forward * speed * timeScale);
	}
}
