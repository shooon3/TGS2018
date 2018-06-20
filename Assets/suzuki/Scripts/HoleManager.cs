using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoleManager : MonoBehaviour
{
    enum InfectiousState : int { INDIVIDUAL, LINE, CROSS };
    [SerializeField]
    InfectiousState state;

    Hole[] hole;
    [SerializeField]
    HoleManager[] manager;          //全列の管理スクリプト
    int myNumber;                   //列番号
    [SerializeField]
    int MaxHp = 1000;
    [SerializeField]
    int decreaseHP = 50;            //ダメージ数
    [SerializeField]
    float flashTime = 0.1f;         //点滅時間
    [SerializeField]
    float invincibleTime = 0.5f;    //無敵時間
    [SerializeField]
    float TimeLag = 0.2f;           //隣との時間差
    [SerializeField]
    float EmergeTime = 0.1f;        //パンプきんの出現(消滅)ラグ
    [SerializeField]
    float radius = 0.1f;            //判定範囲の大きさ

    [SerializeField]
    Material nextColor;     //変更色(仮)

    //[SerializeField]
    //Image hatake;
    [SerializeField]
    GameObject Lane;
    int OverallHP;

    public int MaxHP
    {
        get { return MaxHp;}
        private set { MaxHp = value; }
    }

    public int DecreaseHP
    {
        get { return decreaseHP; }
        private set { decreaseHP = value; }
    }

    public float FlashTime
    {
        get { return flashTime; }
        private set { flashTime = value; }
    }

    public float InvincibleTime
    {
        get { return invincibleTime; }
        private set { invincibleTime = value; }
    }

    public Material NextColor
    {
        get { return nextColor; }
        private set { nextColor = value; }
    }

    void Start ()
    {
        //初期値
        OverallHP = MaxHp;
        //hatake.color = new Color(1, 1, 1, 0);

        //全HoleManagerを取得
        manager = new HoleManager[transform.parent.childCount];
        for(int i = 0; i < manager.Length; i++)
        {
            manager[i] = transform.parent.GetChild(i).GetComponent<HoleManager>();
        }

        //何列目か検索
        int num = 0;
        while (num < manager.Length)
        {
            if (GetComponent<HoleManager>() == manager[num])
            {
                myNumber = num;
                break;
            }
            num++;
        }

        //子オブジェクトの穴を取得
        hole = new Hole[transform.childCount];
        for (int i = 0; i < hole.Length; i++)
        {
            hole[i] = transform.GetChild(i).GetComponent<Hole>();
            //hole[i].SetColider(radius);
        }

        //パンプきんを非表示
        for (int i = 0; i < 10; i++)
        {
            Lane.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 演出の時間差を計算
    /// </summary>
    /// <param name="obj_hole"></param>
    /// <param name="hp"></param>
    public void Unification(Hole obj_hole, int hp)
    {
        int num = 0;
        float f;
        bool b = obj_hole.Infection;
        
        while (num < hole.Length)
        {
            //何個目の穴か検索
            if (obj_hole == hole[num])
            {
                break;
            }
            num++;
        }

        switch (state)
        {
            //個別
            case InfectiousState.INDIVIDUAL:
                if (hp > 0 && MaxHp > hp)
                {
                    hole[num].Flash(0, b);
                }
                else
                {
                    hole[num].Invasion(0, b);
                }
                break;

            //列
            case InfectiousState.LINE:
                //同レーンの他の穴も
                f = 0;
                for (int i = num; i < hole.Length; i++)
                {
                    //時間差を渡す(右方向)
                    if (hp > 0 && MaxHp > hp)
                    {
                        hole[i].Flash(f, b);
                    }
                    else
                    {
                        hole[i].Invasion(f, b);
                    }
                    f += TimeLag;
                }
                f = TimeLag;
                for (int i = num - 1; i >= 0; i--)
                {
                    //時間差を渡す(左方向)
                    if (hp > 0 && MaxHp > hp)
                    {
                        hole[i].Flash(f, b);
                    }
                    else
                    {
                        hole[i].Invasion(f, b);
                    }
                    f += TimeLag;
                }
                break;

            //十字
            case InfectiousState.CROSS:
                //同レーンの他の穴も
                f = 0;
                for (int i = num; i < (num + 2) && i < hole.Length; i++)
                {
                    //時間差を渡す(右方向)
                    if (hp > 0 && MaxHp > hp)
                    {
                        hole[i].Flash(f, b);
                    }
                    else
                    {
                        hole[i].Invasion(f, b);
                    }
                    f += TimeLag;
                }
                f = TimeLag;
                if (num > 0)
                {
                    //時間差を渡す(左方向)
                    if (hp > 0 && MaxHp > hp)
                    {
                        hole[num - 1].Flash(f, b);
                    }
                    else
                    {
                        hole[num - 1].Invasion(f, b);
                    }
                }
                if (myNumber > 0)
                {
                    //時間差を渡す(上方向)
                    if (hp > 0 && MaxHp > hp)
                    {
                        manager[myNumber - 1].hole[num].Flash(f, b);
                    }
                    else
                    {
                        manager[myNumber - 1].hole[num].Invasion(f, b);
                    }
                }
                if (myNumber < manager.Length - 1)
                {
                    //時間差を渡す(下方向)
                    if (hp > 0 && MaxHp > hp)
                    {
                        manager[myNumber + 1].hole[num].Flash(f, b);
                    }
                    else
                    {
                        manager[myNumber + 1].hole[num].Invasion(f, b);
                    }
                }
                break;
        }

        //感染・除染の可視化
        if (!b)
        {
            //感染率を可視化
            if (OverallHP > hp)
            {
                OverallHP = hp;
                float overallHP = OverallHP;
                f = 1 - overallHP / MaxHP;

                ////徐々に色を変える(パターン1)
                //hatake.color = new Color(1, 1, 1, f);

                ////パンプきんを出現(パターン2)
                //int i = Mathf.FloorToInt(f * 10 - 1);//-1,-1,0,0,1,1.....
                //if (i > -1 && !Lane.transform.GetChild(i).gameObject.activeSelf)
                //{
                //    Lane.transform.GetChild(i).gameObject.SetActive(true);
                //}
                
                //パンプきんを出現(パターン3)
                switch (state)
                {
                    case InfectiousState.LINE:
                        if (hp <= 0)
                        {
                            StartCoroutine(Emerge());
                        }
                        break;

                    case InfectiousState.CROSS:
                        for (int i = 0; i < hole.Length; i++)
                        {
                            if (hole[i].Infection)
                            {

                            }
                            else
                            {
                                break;
                            }

                            if (i == hole.Length - 1)
                            {
                                StartCoroutine(Emerge());
                            }
                        }
                        break;
                }
            }
        }
        else
        {
            //除染率を可視化
            if (OverallHP < hp)
            {
                OverallHP = hp;
                float overallHP = OverallHP;
                f = 1 - overallHP / MaxHP;

                ////徐々に元色に戻す(パターン1)
                //hatake.color = new Color(1, 1, 1, f);

                ////パンプきんを消滅(パターン2)
                //int i = Mathf.CeilToInt(f * 10);//10,10,9,9,8,8.....
                //if (i < 10 && Lane.transform.GetChild(i).gameObject.activeSelf)
                //{
                //    Lane.transform.GetChild(i).gameObject.SetActive(false);
                //}

                //パンプきんを消滅(パターン3)
                if (hp >= MaxHp)
                {
                    for(int i = 0; i < manager.Length; i++)
                    {
                        manager[i].Recovery();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 消滅中継用
    /// </summary>
    public void Recovery()
    {
        StartCoroutine(Disappear());
    }

    /// <summary>
    /// ミニかぼちゃ出現
    /// </summary>
    /// <returns></returns>
    IEnumerator Emerge()
    {
        for (int i = 0; i < Lane.transform.childCount; i++)
        {
            Lane.transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(EmergeTime);
        }
    }

    /// <summary>
    /// ミニかぼちゃ消滅
    /// </summary>
    /// <returns></returns>
    IEnumerator Disappear()
    {
        for (int i = Lane.transform.childCount - 1; i >= 0; i--)
        {
            Lane.transform.GetChild(i).gameObject.SetActive(false);
            yield return new WaitForSeconds(EmergeTime);
        }
    }
}
