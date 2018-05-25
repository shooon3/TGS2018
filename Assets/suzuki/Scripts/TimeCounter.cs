using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    bool fade;  //カウントフェード

    [SerializeField]
    Image image;                        //表示スプライト
    [SerializeField]
    Sprite[] sprites = new Sprite[4];   //カウントダウン

    [SerializeField]
    Text timeText;      //TIME表示
    float timer;        //経過時間
    float startTime;    //開始時間

    [SerializeField]
    Pose pose;  //ポーズスクリプト

	void Start ()
    {
        fade = false;
        timeText.gameObject.SetActive(false);

        //カウントダウン開始
        StartCoroutine(StartCount());
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

        //時間経過
        timer = Time.time - startTime;                  //経過時間を計算
        int minuteTime = Mathf.FloorToInt(timer) / 60;  //分を割り出す

        //経過時間を表示
        if(Mathf.FloorToInt(timer) - minuteTime * 60 < 10)
        {
            //秒数が0～9(1桁)の時
            timeText.text = "Time\n" + minuteTime + ":0" + (Mathf.FloorToInt(timer) - minuteTime * 60);
        }
        else
        {
            //秒数が10～59(2桁)の時
            timeText.text = "Time\n" + minuteTime + ":" + (Mathf.FloorToInt(timer) - minuteTime * 60);
        }
        
    }

    /// <summary>
    /// カウントダウン
    /// </summary>
    /// <returns></returns>
    IEnumerator StartCount()
    {
        yield return new WaitForSeconds(1);

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
        timeText.gameObject.SetActive(true);    //経過時間を表示
        pose.GameStart();                       //ポーズボタンを有効化
        yield return new WaitForSeconds(1);

        //カウントの表示を消す
        image.color = new Color(1, 1, 1, 0);
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
}
