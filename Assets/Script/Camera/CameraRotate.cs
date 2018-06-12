using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour {

    Vector3 camAngle;

    public float speed;

	// Use this for initialization
	void Start ()
    {

        camAngle = transform.localEulerAngles;

    }
	
	// Update is called once per frame
	void Update ()
    {

        RotationCam();

    }

    public void RightButton()
    {
        camAngle.y += 120;
    }

    public void LeftButton()
    {
        camAngle.y -= 120;
    }

    void RotationCam()
    {
        float step = speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(camAngle.x, camAngle.y, camAngle.z), step);
    }
}
