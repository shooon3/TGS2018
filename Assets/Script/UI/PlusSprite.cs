using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlusSprite : MonoBehaviour {

    GameObject front;

    float timer;

    Color spColor;

    // Use this for initialization
    void Start () {
        front = Camera.main.gameObject;

        spColor = transform.GetChild(0).GetComponent<SpriteRenderer>().color;

        transform.LookAt(front.transform.position);

        //AttackRotation();
    }

	
	// Update is called once per frame
	void Update () {


		timer += Time.deltaTime;

        if (timer >= 0.7f)
        {
            spColor.a -= 0.1f;
            spColor.a = Mathf.Clamp(spColor.a, 0, 1);
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = spColor;
        }
    }
}
