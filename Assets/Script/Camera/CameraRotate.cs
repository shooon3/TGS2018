﻿using System.Collections;
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

    public float speed;

    public Image clearImg;

    [Header("クリアのスプライト"),NamedArrayAttribute(new string[] {"じゃがいも畑制圧","だいこん畑制圧","とうがらし畑制圧","すべての畑を制圧" })]
    public Sprite[] clearSp;

    [Header("ボタン"),NamedArrayAttribute(new string[] {"右のボタン","左のボタン" })]
    public Button[] button;

    public HoleType HoleType { get; private set; }

    [Header("畑"), NamedArrayAttribute(new string[] {"じゃがいも","だいこん","とうがらし" })]
    public HoleInfection[] holeInf;

    public TimeCounter counter;

    bool isFirst = true;

    int holeInfectionCount = 0;

    int typeIndex;

    int type;

    HoleType rightType, leftType;

    Vector3 camAngle;

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

        HoleCamButton();
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
        //if (holeInf[0].AllInfection() && holeInfectionCount == 0)
        //{
        //    holeInfectionCount++;
        //    clearImg.sprite = clearSp[0];
        //    clearImg.gameObject.SetActive(true);

        //    ButtonDisplay(true);

        //    StartCoroutine(WaitTime());
        //}
        //else if(holeInf[1].AllInfection() && holeInfectionCount == 1)
        //{
        //    holeInfectionCount++;
        //    clearImg.sprite = clearSp[1];
        //    clearImg.gameObject.SetActive(true);

        //    ButtonDisplay(true);

        //    StartCoroutine(WaitTime());
        //}
        //else if(holeInf[2].AllInfection() && holeInfectionCount == 2)
        //{
        //    clearImg.sprite = clearSp[2];
        //    clearImg.gameObject.SetActive(true);

        //    ButtonDisplay(true);

        //    StartCoroutine(WaitTime(true));
        //}

        for(int i = 0; i < holeInf.Length; i++)
        {
            if(holeInf[i].AllInfection() && holeInfectionCount == i)
            {
                holeInfectionCount++;
                clearImg.sprite = clearSp[i];
                clearImg.gameObject.SetActive(true);

                ButtonDisplay(true);

                isFirst = true;

                StartCoroutine(WaitTime());
            }
        }
    }

    /// <summary>
    /// ゲーム中は表示を消す
    /// </summary>
    void HoleCamButton()
    {
        for(int i = 0; i < holeInf.Length; i++)
        {
            if(holeInfectionCount == i && (int)HoleType == i && !holeInf[i].AllInfection() && isFirst)
            {
                StartCoroutine(counter.StartCount());
                ButtonDisplay(false);
                isFirst = false;
            }
        }
    }

    /// <summary>
    /// ボタン二つの表示と非表示
    /// </summary>
    /// <param name="isDisplay"></param>
    void ButtonDisplay(bool isDisplay)
    {
        button[0].gameObject.SetActive(isDisplay);
        button[1].gameObject.SetActive(isDisplay);
    }

    /// <summary>
    /// 待機
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitTime(bool isFinish = false)
    {
        counter.IsStart = false;

        yield return new WaitForSeconds(3.0f);

        counter.IsStart = true;
        clearImg.gameObject.SetActive(false);

        if (isFinish == false) yield break;

        clearImg.sprite = clearSp[3];
        clearImg.gameObject.SetActive(true);
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
