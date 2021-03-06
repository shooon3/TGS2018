﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ボムのタイプ
/// </summary>
public enum BomType
{
    normal = 0,
    special
}

/// <summary>
/// 出せるボムの数を制御するクラス
/// </summary>
public class BomCount : MonoBehaviour {


    public List<BomType> bomList = new List<BomType>();

    public Text bombCountText;

    public TimeCounter counter;

    const int ConvertionConstant = 65248;

    BomType bomType;

    int maxBomNum; //最大ボム数

	// Use this for initialization
	void Start () {

        maxBomNum = GetComponent<MinionCreate>().maxBom;

        for (int i = 0; i < maxBomNum; i++)
        {
            bomList.Add(BomType.normal);
        }
    }
	
	// Update is called once per frame
	void Update () {
        bombCountText.text = ConvertToFullWidth(NowBomCount().ToString());
    }

    /// <summary>
    /// ボムを増やす
    /// </summary>
    public void AddBom(BomType type)
    {
        if (counter.IsStart == false) return;
        bomList.Add(type);
    }

    /// <summary>
    /// ボムを使う(ボムの数を減らす)
    /// </summary>
    public void UseBom()
    {
        bomList.RemoveAt(0);
    }

    public void ClearAddBomb()
    {
        for (int i = 0; bomList.Count < 16; i++)
        {
            bomList.Add(BomType.normal);
        }
    }

    /// <summary>
    /// 次のボムの種類
    /// </summary>
    /// <returns></returns>
    public BomType NextBomType()
    {
        return bomList[0];
    }

    /// <summary>
    /// 現在のボムの数
    /// </summary>
    /// <returns></returns>
    public int NowBomCount()
    {
        return bomList.Count;
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
