using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pose : MonoBehaviour
{
    [SerializeField]
    Image filterImage;
    [SerializeField]
    Button poseButton;

	void Start ()
    {
        filterImage.gameObject.SetActive(false);
	}

    /// <summary>
    /// ポーズ
    /// </summary>
    public void OnClick()
    {
        //ゲームを一時的に止める
        Time.timeScale = 0;

        //ボタンを無効化
        poseButton.interactable = false;

        //ポーズメニュー表示
        filterImage.gameObject.SetActive(true);
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
        //ボタンの無効化を解除
        poseButton.interactable = true;

        //ゲームを再開する
        filterImage.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
