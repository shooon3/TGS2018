﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BtnMaster : MonoBehaviour
{
    public string backReStart;
    public string backTitle;

    public GameObject BlackMax;
    public GameObject yesBtn;
    public GameObject noBtn;

    void Start()
    {
        BlackMax.SetActive(false);
        yesBtn.SetActive(false);
        noBtn.SetActive(false);
    }

    public void ReStart1()
    {
        BlackMax.SetActive(true);
        yesBtn.SetActive(true);
        noBtn.SetActive(true);
    }

    public void TitleBack1()
    {
        BlackMax.SetActive(true);
        yesBtn.SetActive(true);
        noBtn.SetActive(true);
    }

    public void Yes()
    {
        //SceneManager.LoadScene("");
    }

    public void No()
    {
        BlackMax.SetActive(false);
        yesBtn.SetActive(false);
        noBtn.SetActive(false);
    }

}