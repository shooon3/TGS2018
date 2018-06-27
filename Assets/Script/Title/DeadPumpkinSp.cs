using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPumpkinSp : MonoBehaviour {

    const int CREATE_RANGE_X = 90;
    const int CREATE_RANGE_Y = 60;
    const int CREATE_RANGE_Z = 40;

    public GameObject pumpkinSp;

    float timer = 0;

	// Use this for initialization
	void Start () {
		
	}
	


	// Update is called once per frame
	void Update () {
        Timer();
	}

    void Timer()
    {
        timer += Time.deltaTime;
        if(timer >= 1.0f)
        {
            int random_x = Random.Range(-CREATE_RANGE_X, CREATE_RANGE_X);
            int random_y = Random.Range(0, CREATE_RANGE_Y);
            int random_z = Random.Range(-CREATE_RANGE_Z, CREATE_RANGE_Z);

            Vector3 vec = new Vector3(random_x, random_y, random_z);

            Instantiate(pumpkinSp, vec, Quaternion.identity);

            timer = 0;
        }
    }
}
