using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum HoleType
{
    _potato = 0,
    _radish,
    _pepper
}

public class CameraRotate : MonoBehaviour {

    Vector3 camAngle;

    public float speed;

    int type;

    public Text crealText;

    public HoleType HoleType { get; private set; }

    [Header("畑"), NamedArrayAttribute(new string[] {"じゃがいも","だいこん","とうがらし" })]
    public HoleInfection[] holeInf;

    public TimeCounter counter;

    int holeInfectionCount = 0;

    int typeIndex;

    HoleType rightType, leftType;

	// Use this for initialization
	void Start ()
    {
        AudioManager.Instance.PlayBGM("gameBGM");
        camAngle = transform.localEulerAngles;

    }
	
	// Update is called once per frame
	void Update ()
    {

        RotationCam();
        InfectionCheck();
        HoleTypeSet();

    }

    public void RightButton()
    {
        if (holeInfectionCount == 0) return;

        if (holeInfectionCount == 1 && rightType == HoleType._pepper) return;

        camAngle.y += 120;

        type++;
    }

    public void LeftButton()
    {
        if (holeInfectionCount == 0) return;

        if (holeInfectionCount == 1 && leftType == HoleType._pepper) return;

        camAngle.y -= 120;

        type--;
    }

    void RotationCam()
    {
        float step = speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(camAngle.x, camAngle.y, camAngle.z), step);
    }

    void InfectionCheck()
    {
        if (holeInf[0].AllInfection() && holeInfectionCount == 0)
        {
            holeInfectionCount++;
            crealText.text = "じゃがいも畑を制圧した！";
            crealText.gameObject.SetActive(true);

            StartCoroutine(WaitTime());
        }
        else if(holeInf[1].AllInfection() && holeInfectionCount == 1)
        {
            holeInfectionCount++;
            crealText.text = "だいこん畑を制圧した！";
            crealText.gameObject.SetActive(true);

            StartCoroutine(WaitTime());
        }
        else if(holeInf[2].AllInfection() && holeInfectionCount == 2)
        {
            crealText.text = "すべての畑を制圧した！";
            crealText.gameObject.SetActive(true);
        }
    }

    IEnumerator WaitTime()
    {
        counter.IsStart = false;

        yield return new WaitForSeconds(3.0f);

        counter.IsStart = true;
        crealText.gameObject.SetActive(false);
    }

    void HoleTypeSet()
    {
        if (type > 2 || type < -2) type = 0;

        if (type == 0) HoleType = HoleType._potato;
        else if (type == -1 || type == 2) HoleType = HoleType._radish;
        else if (type == 1 || type == -2) HoleType = HoleType._pepper;

        int left = (int)HoleType + 1;
        int right = (int)HoleType - 1;

        if (right < 0) right = 2;
        if (left > 2) left = 0;

        rightType = (HoleType)Enum.ToObject(typeof(HoleType), right);
        leftType = (HoleType)Enum.ToObject(typeof(HoleType), left);
    }
}
