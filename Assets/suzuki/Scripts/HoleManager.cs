using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    Hole[] hole;
    [SerializeField]
    int hp = 1000;
    [SerializeField]
    int decreaseHP = 50;            //ダメージ数
    [SerializeField]
    float flashTime = 0.1f;         //点滅時間
    [SerializeField]
    float invincibleTime = 0.5f;    //無敵時間
    [SerializeField]
    float TimeLag = 0.2f;
    [SerializeField]
    float radius = 0.1f;

    [SerializeField]
    Material nextColor;     //変更色(仮)

    public int HP
    {
        get { return hp; }
        private set { hp = value; }
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
        //子オブジェクトの穴を取得
        hole = new Hole[transform.childCount];
        for (int i = 0; i < hole.Length; i++)
        {
            hole[i] = transform.GetChild(i).GetComponent<Hole>();
            hole[i].SetColider(radius);
        }
	}

    public void Unification(Hole obj_hole, int hp)
    {
        int num = 0;
        while (num < hole.Length)
        {
            //何個目の穴か検索
            if (obj_hole == hole[num])
            {
                break;
            }
            num++;
        }

        //同レーンの他の穴も
        float f = 0;
        for (int i = num; i < hole.Length; i++)
        {
            //時間差を渡す(右方向)
            if (hp > 0)
            {
                hole[i].Flash(f);
            }
            else
            {
                hole[i].Invasion(f);
            }
            f += TimeLag;
        }
        f = TimeLag;
        for (int i = num - 1; i >= 0; i--)
        {
            //時間差を渡す(左方向)
            if (hp > 0)
            {
                hole[i].Flash(f);
            }
            else
            {
                hole[i].Invasion(f);
            }
            f += TimeLag;
        }
    }
}
