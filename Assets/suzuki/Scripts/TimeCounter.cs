﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    bool fade;  //カウントフェード
    bool clear; //クリア判定
    bool isFirst = true;

    [SerializeField]
    Image image;                        //表示スプライト
    [SerializeField]
    Sprite[] sprites = new Sprite[4];   //カウントダウン

    [SerializeField]
    Text timeText;      //TIME表示
    float timer;        //経過時間
    float startTime;    //開始時間

    static int clearTime;      //クリア時間

    int index = 0;


    [SerializeField]
    Pose pose;  //ポーズスクリプト

    [SerializeField]
    GameObject clearText;

    [Header("操作説明")]
    public Image operationImg;

    [Header("操作説明画像")]
    public Sprite[] operationSp;

    [Header("ゲームオーバー表示用Image")]
    public Image gameOverImg;

    [Header("ゲームオーバースプライト")]
    public Sprite gameoverSp;

    public HoleInfection Hole;

    public BossPumpKing pumpking;

    /// <summary>
    /// ゲームはスタートしているかどうか
    /// </summary>
    public bool IsStart { get; set; }

    public bool IsClear { get; set; }

    public bool IsGameOver { get; set; }

    public float Timer
    {
        get { return timer; }
        set { timer = value; }
    }

	void Start ()
    {
        fade = false;
        clear = false;
        IsStart = false; //変更
        timeText.gameObject.SetActive(false);

        ////カウントダウン開始
        //StartCoroutine(StartCount());
	}
	
	void Update ()
    {
        //フェードアウト
        if (fade)
        {
            if (image.color.a > 0)
            {
                image.color -= new Color(0, 0, 0, Time.deltaTime * 2);
            }
            else
            {
                fade = false;
            }
        }

        //クリアタイム記録
        if (!IsClear || !IsGameOver)
        {
            //時間経過
            if(IsStart) timer += Time.deltaTime;                  //経過時間を計算
            int minuteTime = Mathf.FloorToInt(timer) / 60;  //分を割り出す

            //経過時間を表示
            if (Mathf.FloorToInt(timer) - minuteTime * 60 < 10)
            {
                //秒数が0～9(1桁)の時
                timeText.text = "Time\n" + minuteTime + ":0" + (Mathf.FloorToInt(timer) % 60);
            }
            else
            {
                //秒数が10～59(2桁)の時
                timeText.text = "Time\n" + minuteTime + ":" + (Mathf.FloorToInt(timer) % 60);
            }


            //クリア処理
            if (Hole.enabled && Hole.AllInfection() && IsClear == false)    //クリアテキストがアクティブ
            {
                //clearText.SetActive(true);
                //ステージクリア
                clearTime = Mathf.FloorToInt(timer);    //クリアタイムを秒で記録
                IsClear = true;

                //ログにクリアタイムを表記
                if (Mathf.FloorToInt(timer) - minuteTime * 60 < 10)
                {
                    //秒数が0～9(1桁)の時
                    Debug.Log("クリアタイム " + clearTime / 60 + ":0" + clearTime % 60);
                }
                else
                {
                    //秒数が10～59(2桁)の時
                    Debug.Log("クリアタイム " + clearTime / 60 + ":" + clearTime % 60);
                }

                //ポーズボタン機能停止
                pose.GameClear();

                //リザルトへ移動
                StartCoroutine(GoResult());
            }

            //ゲームオーバー処理
            if(pumpking.HP <= 0 && IsGameOver == false)
            {
                IsGameOver = true;

                gameOverImg.sprite = gameoverSp;
                gameOverImg.gameObject.SetActive(true);

                //ポーズボタン機能停止
                pose.GameClear();

                //リザルトへ移動
                StartCoroutine(GoGameOver());
            }
        }
    }
    
    /// <summary>
    /// クリアタイム(秒単位)
    /// </summary>
    /// <returns></returns>
    public static int GetClearTime()
    {
        return clearTime;
    }

    /// <summary>
    /// カウントダウン
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartCount()
    {

        if(isFirst)
        {
            StartCoroutine(FirstOperation());
        }

        yield return new WaitWhile(() => isFirst);

        //カウント3
        Indicate(3);
        yield return new WaitForSeconds(0.5f);
        fade = true;    //表示から半秒後徐々に透明化
        yield return new WaitForSeconds(0.5f);

        //カウント2
        Indicate(2);
        yield return new WaitForSeconds(0.5f);
        fade = true;
        yield return new WaitForSeconds(0.5f);

        //カウント1
        Indicate(1);
        yield return new WaitForSeconds(0.5f);
        fade = true;
        yield return new WaitForSeconds(0.5f);

        //ゲームスタート
        Indicate(0);
        startTime = Time.time;                  //開始時間を設定
        //timeText.gameObject.SetActive(true);    //経過時間を表示
        pose.GameStart();                       //ポーズボタンを有効化

        IsStart = true;

        yield return new WaitForSeconds(1);

        //カウントの表示を消す
        image.color = new Color(1, 1, 1, 0);
    }

    /// <summary>
    /// ゲーム開始時の説明
    /// </summary>
    IEnumerator FirstOperation()
    {
#if UNITY_EDITOR

#else
        FadeReceiveEvent fadeEvent = FindObjectOfType<FadeReceiveEvent>();

        yield return new WaitWhile( () => !fadeEvent.IsAnimEnd);
#endif

        operationImg.sprite = operationSp[0];
        operationImg.gameObject.SetActive(true);

        while (index + 1 != operationSp.Length)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                index++;
                operationImg.sprite = operationSp[index];
            }
            yield return null;
        }

        yield return new WaitWhile(() => index + 1 != operationSp.Length);

        isFirst = false;

        operationImg.gameObject.SetActive(false);

    }

    /// <summary>
    /// カウント表示
    /// </summary>
    /// <param name="i"></param>
    void Indicate(int i)
    {
        fade = false;
        image.color = new Color(1, 1, 1, 1);

        //サイズ調整
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sprites[i].bounds.size.x, sprites[i].bounds.size.y) * 100;

        //スプライトを表示する
        image.sprite = sprites[i];
    }

    /// <summary>
    /// シーン遷移
    /// </summary>
    /// <returns></returns>
    IEnumerator GoResult()
    {
        yield return new WaitForSeconds(3);
        Time.timeScale = 1;
        //SceneManager.LoadScene((int)GameMode.Mode.RESULT);
        FadeManager.Instance.LoadSpriteScene("Result", 1.0f, false);
    }

    /// <summary>
    /// シーン遷移
    /// </summary>
    /// <returns></returns>
    IEnumerator GoGameOver()
    {
        yield return new WaitForSeconds(3);
        Time.timeScale = 1;
        FadeManager.Instance.LoadSpriteScene("gameOver", 1.0f, false);
    }
}
