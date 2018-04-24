using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    Hole[] hole;
    
	void Start ()
    {
        //子オブジェクトの穴を取得
        hole = new Hole[transform.childCount];
        for (int i = 0; i < hole.Length; i++)
        {
            hole[i] = transform.GetChild(i).GetComponent<Hole>();
            hole[i].myNumber = i;   //管理番号を振る
        }
	}

    public void Unification(int num)
    {
        //同レーンの他の穴も
        float f1 = 0, f2 = 0.5f;
        for (int i = num; i < hole.Length; i++)
        {
            //時間差を渡す(右方向)
            hole[i].Invasion(f1);
            f1 += f2;
        }
        f1 = f2;
        for (int i = num - 1; i >= 0; i--)
        {
            //時間差を渡す(左方向)
            hole[i].Invasion(f1);
            f1 += f2;
        }
    }
}
