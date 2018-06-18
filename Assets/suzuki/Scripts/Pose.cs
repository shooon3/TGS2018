using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pose : MonoBehaviour
{
    bool game;
    public int pcCost;

    [SerializeField]
    Image filterImage;  //フィルター
    [SerializeField]
    Button poseButton;  //ボタン本体
    Button Left;
    Button Right;

    public Animator animator;

    //各ボタン
    public GameObject LeftBtn;//左の畑へ
    public GameObject RightBtn;//右の畑へ

    //Pause最終決定
    public GameObject BlackMax;//Pause最終決定用暗幕
    public GameObject yesBtn;//最終決定用Yesボタン
    public GameObject noBtn;//最終決定用Noボタン
  　//public GameObject PauseMas;

    //Pauseアニメーションで動くやつ（ここではSetActiveのOnOffを管理する）
    public GameObject Top;
    public GameObject TopChain;
    public GameObject Under;
    public GameObject UnderChain;



    void Start ()
    {
        game = false;
        filterImage.gameObject.SetActive(false);
        poseButton.interactable = false;
        animator.SetBool("PauseShot", false);

        //Pauseが発動したら消すボタン達
        BlackMax.SetActive(false);
        noBtn.SetActive(false);

        pcCost = 0;
    }

    /// <summary>
    /// ポーズ解放
    /// </summary>
    public void GameStart()
    {
        game = true;
        poseButton.interactable = true;
    }

    /// <summary>
    /// ポーズ
    /// </summary>
    public void OnClick()
    {
        if (game)
        {
            //ゲームを一時的に止める
            Time.timeScale = 0;

            //ボタンを無効化
            poseButton.interactable = false;

            //ポーズメニュー表示
            filterImage.gameObject.SetActive(true);
            animator.SetBool("PauseShot", true);
            animator.SetBool("PauseCancel", false);
            
            //畑の移動ボタン表示Off
            LeftBtn.SetActive(false);
            RightBtn.SetActive(false);

            //タイムスケールを無視する
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }

    public void PauseChange()
    {
        // リスタート
        if (pcCost==0)
        {
            //ポーズを解除
            filterImage.gameObject.SetActive(false);
            Time.timeScale = 1;

            //最初からゲームを始める
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        //タイトルへ戻る
        if (pcCost==1)
        {
            //ポーズを解除
            filterImage.gameObject.SetActive(false);
            Time.timeScale = 1;

            //ステージ選択へ戻る
            SceneManager.LoadScene(0);
        }
    }

    ///// <summary>
    ///// リスタート
    ///// </summary>
    //public void ReStart()
    //{
    //    //ポーズを解除
    //    filterImage.gameObject.SetActive(false);
    //    Time.timeScale = 1;

    //    //最初からゲームを始める
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}

    ///// <summary>
    ///// ステージ選択へ戻る
    ///// </summary>
    //public void BackMenu()
    //{
    //    //ポーズを解除
    //    filterImage.gameObject.SetActive(false);
    //    Time.timeScale = 1;

    //    //ステージ選択へ戻る
    //    SceneManager.LoadScene(0);
    //}

    /// <summary>
    /// 再開
    /// </summary>
    public void Reopening()
    {
        //ボタンの無効化を解除
        poseButton.interactable = true;
        LeftBtn.SetActive(true);
        RightBtn.SetActive(true);

        //タイムスケールを無視する
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        //Pauseメニュー撤退
        animator.SetBool("PauseCancel", true);
        animator.SetBool("PauseShot", false);

        //タイムスケールを無視する
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        //ゲームを再開する
        filterImage.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    /// <summary>
    /// クリア後処理
    /// </summary>
    public void GameClear()
    {
        //ポーズメニューのボタンたちを非表示
        for (int i = 0; i < (filterImage.gameObject.transform.childCount); i++)
        {
            filterImage.gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        filterImage.color = new Color(0, 0, 0, 0);  //フィルターの透明度を調整
        filterImage.gameObject.SetActive(true);     //フィルターを非表示
    }


    // リスタートボタンが押されたら
    public void ReStart1()
    {
        BlackMax.SetActive(true);
        yesBtn.SetActive(true);
        noBtn.SetActive(true);
        //PauseMas.SetActive(false);
        Top.SetActive(false);
        TopChain.SetActive(false);
        Under.SetActive(false);
        UnderChain.SetActive(false);
        pcCost = 0;
    }

    //タイトルへ戻るが押されたら
    public void TitleBack1()
    {
        BlackMax.SetActive(true);
        yesBtn.SetActive(true);
        noBtn.SetActive(true);
        //PauseMas.SetActive(false);
        Top.SetActive(false);
        TopChain.SetActive(false);
        Under.SetActive(false);
        UnderChain.SetActive(false);
        pcCost = 1;
    }

    //Noボタンが押されたら
    public void No()
    {
        BlackMax.SetActive(false);
        yesBtn.SetActive(false);
        noBtn.SetActive(false);
        //PauseMas.SetActive(true);
        Top.SetActive(true);
        TopChain.SetActive(true);
        Under.SetActive(true);
        UnderChain.SetActive(true);
    }


}
