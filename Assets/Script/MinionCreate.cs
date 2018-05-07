﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フリックの左右
/// </summary>
enum FlickState
{
    Filst = 0,
    Left,
    Right
}

public class MinionCreate : MonoBehaviour {

    //-------------------------------------
    // public
    //-------------------------------------

    [Header("パンプキンが増えるフリック数"),NamedArrayAttribute(new string[] {"二個目のパンプキン" , "三個目のパンプキン", "四個目のパンプキン", "五個目のパンプキン", "六個目のパンプキン", "七個目のパンプキン", "八個目のパンプキン", "九個目のパンプキン", "十個目のパンプキン" })]
    public int[] bafferCount;

    [Header("パンプ菌のモデル")]
    public GameObject pumpkinPre;

    [Header("パンプ菌集団の親オブジェクト")]
    public GameObject massParentPre;

    //-------------------------------------
    // private
    //-------------------------------------

    List<GameObject> pumpkinSp = new List<GameObject>(); //パンプキンImage

    GameObject pumpkinParent; //吹き出し(各パンプキンの親)オブジェクト

    float touchNowPosX; //現在のタッチポジション
    float startFlickX; //タッチした直後のポジション(タッチ直後にフリック判定にならないようにするための除外用変数)
    float memoryPos; //1フレーム前の位置を記憶する

    int flickCount; //フリックした回数
    int createCount = 0; //パンプキンを出す数

    FlickState flickState; //フリックされた方向
    FlickState nextFlickState; //次にフリックする方向

    Ray ray;
    RaycastHit hit;

    //-------------------------------------
    // プロパティ
    //-------------------------------------

    public Vector3 CreatePos { get; private set;}
    public bool CreateFlg { get; set; }

    // Use this for initialization
    void Start () {

        pumpkinParent = transform.Find("PlaerManager/TouchPosObj").gameObject;

        for(int i = 0; i < bafferCount.Length; i++)
        {
            pumpkinSp.Add(pumpkinParent.transform.GetChild(i).gameObject);
        }

        FlickInitialize();
    }
	
	// Update is called once per frame
	void Update () {

        FlickInitialize();
        GetTouchPos();
        Flick();
        DisplayPampking();
        PumpkinCreate();

    }

    /// <summary>
    /// タッチした場所を取得
    /// </summary>
    void GetTouchPos()
    {
        touchNowPosX = Input.mousePosition.x;

        if (Input.GetButton("Fire1"))
        {
            FlickSide();
        }
    }

    /// <summary>
    /// パンプキンを表示する
    /// </summary>
    /// <param name="count">表示するパンプキンの数</param>
    void DisplayPampking()
    {
        if (createCount == bafferCount.Length || flickCount != bafferCount[createCount]) return;

        pumpkinSp[createCount].SetActive(true);
        createCount++;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    void FlickInitialize()
    {
        if (Input.GetButtonDown("Fire1") == false) return;

        memoryPos = touchNowPosX;
        startFlickX = Input.mousePosition.x;

        flickCount = 0;
        createCount = 0;

        flickState = FlickState.Filst;
        nextFlickState = FlickState.Filst;

        foreach(Transform obj in pumpkinParent.transform)
        {
            obj.gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// 左右フリックの判定
    /// </summary>
    void FlickSide()
    {
        if (startFlickX == touchNowPosX) return;

        if (memoryPos > touchNowPosX) flickState = FlickState.Left;
        else if (memoryPos < touchNowPosX) flickState = FlickState.Right;

        //１フレーム前のポジションを記憶させる
        memoryPos = touchNowPosX;
    }

    /// <summary>
    /// 次にどちらにフリックするのかを決め、フリックカウントを増やす
    /// </summary>
    void Flick()
    {
        switch (flickState)
        {
            case FlickState.Right:
                if (nextFlickState == FlickState.Right || nextFlickState == 0)
                {
                    nextFlickState = FlickState.Left;
                    flickCount++;
                }
                break;

            case FlickState.Left:
                if (nextFlickState == FlickState.Left || nextFlickState == 0)
                {
                    nextFlickState = FlickState.Right;
                    flickCount++;
                }
                break;
        }
    }

    void PumpkinCreate()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit) == false) return;

            CreatePos = new Vector3(hit.point.x, 0, hit.point.z);

            GameObject parentObj = Instantiate(massParentPre, CreatePos, Quaternion.identity);

            CreateFlg = true;

            for (int i = 0; i < createCount + 1; i++)
            {
                Vector3 position = parentObj.transform.position;
                Vector2 size = new Vector2(4.0f, 4.0f);

                float x = Random.Range(position.x - size.x / 2, position.x + size.x / 2);
                float z = Random.Range(position.z - size.y / 2, position.z + size.y / 2);

                Instantiate(pumpkinPre, new Vector3(x, 0, z), Quaternion.identity, parentObj.transform);
            }
        }
    }
}