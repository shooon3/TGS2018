using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    bool interval;  //処理を一時停止
    bool finish;    //演出処理終了
    int time;       //表示しているTIMEの値
    int ClearTime;  //クリアタイム
    int number;     //検索回数
    int count;      //ループ回数
    [SerializeField]
    int[] rank = { 90, 150, 210 };  //ランク時間割(秒)
    [SerializeField]
    string[] RankStr = { "S", "A", "B", "C" };

    [SerializeField]
    Text timeText;  //時間表示テキスト
    [SerializeField]
    Text rankText;  //ランク表示テキスト

	void Start ()
    {
        //初期値
        interval = false;
        finish = false;
        time = 0;
        number = 0;
        count = 0;

        //クリアタイムを取得(秒単位)
        ClearTime = TimeCounter.GetClearTime();
    }

    void Update()
    {
        //タイム加算表示
        if (time < ClearTime)
        {
            time++;
            //タイムを「分:秒」に変換し表示
            if (time % 60 < 10)
            {
                //秒数が0～9(1桁)の時
                timeText.text = time / 60 + ":0" + time % 60;
            }
            else
            {
                //秒数が10～59(2桁)の時
                timeText.text = time / 60 + ":" + time % 60;
            }
        }
        //ランク表示
        else if (!finish && !interval)
        {
            //ループ演出
            if (count < 10)
            {
                if (number < rank.Length)
                {
                    RankChecker();
                    number++;
                }
                else
                {
                    rankText.text = "C";
                    count++;
                    number = 0;
                }
            }
            //ランク確定
            else
            {
                if (number < rank.Length)
                {
                    RankChecker();
                    if (time < rank[number])
                    {
                        finish = true;
                    }
                    number++;
                }
                else
                {
                    rankText.text = "C";
                    finish = true;
                }
            }
            interval = true;

        }
        //演出間隔
        else
        {
            interval = false;
        }

        //セレクト?画面に戻る
        if (finish && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene((int)GameMode.Mode.SELECT);
        }
    }

    /// <summary>
    /// ランク表示
    /// </summary>
    void RankChecker()
    {
        rankText.text = RankStr[number];
    }
}
