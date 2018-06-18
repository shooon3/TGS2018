using System.Collections;
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
    public GameObject PauseMas;
   

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
        PauseMas.SetActive(false);
    }

    public void TitleBack1()
    {
        BlackMax.SetActive(true);
        yesBtn.SetActive(true);
        noBtn.SetActive(true);
        PauseMas.SetActive(false);
    }

    public void No()
    {
        BlackMax.SetActive(false);
        yesBtn.SetActive(false);
        noBtn.SetActive(false);
        PauseMas.SetActive(true);
    }

}
