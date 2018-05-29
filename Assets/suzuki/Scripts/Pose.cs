using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pose : MonoBehaviour
{
    bool game;

    [SerializeField]
    Image filterImage;  //フィルター
    [SerializeField]
    Button poseButton;  //ボタン本体

	void Start ()
    {
        game = false;
        filterImage.gameObject.SetActive(false);
        poseButton.interactable = false;
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
        }
    }

    /// <summary>
    /// リスタート
    /// </summary>
    public void ReStart()
    {
        //ポーズを解除
        filterImage.gameObject.SetActive(false);
        Time.timeScale = 1;

        //最初からゲームを始める
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// ステージ選択へ戻る
    /// </summary>
    public void BackMenu()
    {
        //ポーズを解除
        filterImage.gameObject.SetActive(false);
        Time.timeScale = 1;

        //ステージ選択へ戻る
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 再開
    /// </summary>
    public void Reopening()
    {
        if (game)
        {
            //ボタンの無効化を解除
            poseButton.interactable = true;

            //ゲームを再開する
            filterImage.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
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
}
