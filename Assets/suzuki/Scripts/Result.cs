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
    int intervalTime;
    [SerializeField]
    int[] rank = { 60, 90, 150 };  //ランク時間割(秒)
    [SerializeField]
    string[] RankStr = { "S", "A", "B", "C" };

    [SerializeField]
    Text timeText;  //時間表示テキスト
    [SerializeField]
    Text rankText;  //ランク表示テキスト
    [SerializeField]
    Button[] buttons = new Button[2];   //シーン移行用ボタン

    const int ConvertionConstant = 65248;

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
            //演出スキップ
            if (Input.GetMouseButtonDown(0))
            {
                time = ClearTime;
            }
            //通常時
            else
            {
                //タイム加算演出
                time++;
            }

            //タイムを「分:秒」に変換し表示
            if (time % 60 < 10)
            {
                //秒数が0～9(1桁)の時
                timeText.text = ConvertToFullWidth(time / 60 + ":0" + time % 60);
            }
            else
            {
                //秒数が10～59(2桁)の時
                timeText.text = ConvertToFullWidth(time / 60 + ":" + time % 60);
            }
        }
        //ランク表示
        else if (!finish)
        {
            //演出スキップ
            if (Input.GetMouseButtonDown(0))
            {
                for(int i = 0; i < rank.Length; i++)
                {
                    if (time < rank[i])
                    {
                        rankText.text = RankStr[i];
                        ResultFinish();
                        break;
                    }
                }
            }
            //通常時
            else if(!interval)
            {
                //ループ演出
                if (count < 10)
                {
                    //ランクを順に表示
                    if (number < RankStr.Length)
                    {
                        RankChecker();
                        number++;
                    }
                    //ループ
                    else
                    {
                        count++;
                        number = 0;
                    }
                }
                //ランク確定
                else
                {
                    RankChecker();
                    if (number < rank.Length)
                    {
                        if (time < rank[number])
                        {
                            ResultFinish();
                        }
                        number++;
                    }
                    else
                    {
                        ResultFinish();
                    }
                }
                interval = true;

                //演出間隔
                StartCoroutine(SlotInterval(intervalTime));
            }
        }
    }

    /// <summary>
    /// ランク表示
    /// </summary>
    void RankChecker()
    {
        if (number < RankStr.Length)
        {
            rankText.text = RankStr[number];
        }
        else
        {
            rankText.text = "圏外";
        }
    }

    /// <summary>
    /// frameごとに書き換える
    /// </summary>
    /// <param name="frame"></param>
    /// <returns></returns>
    IEnumerator SlotInterval(int frame)
    {
        //1フレーム待つ
        while (frame > 0)
        {
            yield return null;
            frame--;
        }

        interval = false;
    }

    /// <summary>
    /// リザルト処理終了
    /// </summary>
    void ResultFinish()
    {
        finish = true;
        for (int i = 0; i < buttons.Length; i++)
        {
            //ボタンを表示する
            buttons[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// リトライ
    /// </summary>
    public void Retry()
    {
        FadeManager.Instance.LoadScene("main", 1.5f, false);
        ////ゲームを始める
        //if (finish)
        //{
        //    SceneManager.LoadScene((int)GameMode.Mode.GAME);
        //}
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    public void End()
    {
        FadeManager.Instance.LoadScene("title", 1.5f, false);
        ////セレクト?画面に戻る
        //if (finish)
        //{
        //    SceneManager.LoadScene((int)GameMode.Mode.SELECT);
        //}
    }

    public string ConvertToFullWidth(string halfWidthStr)
    {
        string fullWidthStr = null;

        for (int i = 0; i < halfWidthStr.Length; i++)
        {
            fullWidthStr += (char)(halfWidthStr[i] + ConvertionConstant);
        }

        return fullWidthStr;
    }
}
