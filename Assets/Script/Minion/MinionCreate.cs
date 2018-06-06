using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// フリックの左右
/// </summary>
enum FlickState
{
    Filst = 0,
    Left,
    Right
}

[RequireComponent(typeof(BomCount))]
public class MinionCreate : MonoBehaviour {

    //-------------------------------------
    // public
    //-------------------------------------

    [Header("パンプキンが増えるフリック数"),NamedArrayAttribute(new string[] {"二個目のパンプキン" , "四個目のパンプキン",  "八個目のパンプキン"})]
    public int[] bafferCount;

    [Header("戦うパンプ菌")]
    public GameObject pumpkinPre;

    [Header("追従するだけのパンプ菌")]
    public GameObject displayPumpkinPre;

    [Header("パンプ菌集団の親オブジェクト")]
    public GameObject massParentPre;

    [Header("パンプキング")]
    public GameObject pumpking;

    [Header("ボムの最大数")]
    public int maxBom;

    [Header("パンプキン生成時のエフェクト")]
    public GameObject createEffect;

    [Header("パンプキン分裂画像"), NamedArrayAttribute(new string[] { "一個目のパンプキン","二個目のパンプキン", "四個目のパンプキン", "八個目のパンプキン" })]
    public Sprite[] pumpkinsSp; //パンプキンImage

    [Header("パンプキン吹き出し")]
    public SpriteRenderer pumpRender;

    [Header("パンプキン"), NamedArrayAttribute(new string[] { "一個目のパンプキン", "二個目のパンプキン", "四個目のパンプキン", "八個目のパンプキン" })]
    public VegetableStatus[] status;

    [Header("カメラ")]
    public CameraShake camShake;


    //-------------------------------------
    // private
    //-------------------------------------

    GameObject pumpkinParent; //吹き出し(各パンプキンの親)オブジェクト
    GameObject minionParent;
    GameObject attackPumpkin; //攻撃するパンプキン

    MinionManager minionMar;
    ThrowBom throwBom;
    BomCount bomCount;
    PlayerMove playerMove;

    float touchNowPosX; //現在のタッチポジション
    float startFlickX; //タッチした直後のポジション(タッチ直後にフリック判定にならないようにするための除外用変数)
    float memoryPos; //1フレーム前の位置を記憶する

    int flickCount; //フリックした回数
    int displayCount; //パンプキンを出す数
    int beforeCount;
    int flickIndex;

    FlickState flickState; //フリックされた方向
    FlickState nextFlickState; //次にフリックする方向

    // Use this for initialization
    void Start () {

        pumpkinParent = transform.GetChild(0).gameObject;

        pumpkinParent.SetActive(false);

        throwBom = pumpking.GetComponent<ThrowBom>();
        bomCount = GetComponent<BomCount>();
        playerMove = GetComponent<PlayerMove>();

        pumpRender.sprite = pumpkinsSp[0];

        FlickInitialize();
    }
	
	// Update is called once per frame
	void Update () {

        if (playerMove.IsStart != true) return;

        GetTouchPos();
        Flick();
        DisplayPampking();
        CreatePos();

        //パンプ菌が生成できるようになったら(爆弾が地面に衝突してたら)
        if (minionMar != null && minionMar.IsMinionCreate)
        {
            MinionsCreate(minionParent);
            minionMar.IsMinionCreate = false;
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    void FlickInitialize()
    {
        memoryPos = touchNowPosX;
        startFlickX = Input.mousePosition.x;

        flickCount = 0;
        flickIndex = 0;
        displayCount = 1;

        flickState = FlickState.Filst;
        nextFlickState = FlickState.Filst;

        foreach (Transform obj in pumpkinParent.transform)
        {
            obj.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// タッチした場所を取得
    /// </summary>
    void GetTouchPos()
    {
        touchNowPosX = Input.mousePosition.x;

        if (playerMove.IsTouch)
        {
            FlickSide();
            pumpkinParent.SetActive(true);
        }
        if (!Input.GetButton("Fire1")) pumpkinParent.SetActive(false);
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

    /// <summary>
    /// パンプキンを表示する
    /// </summary>
    /// <param name="count">表示するパンプキンの数</param>
    void DisplayPampking()
    {
        if (flickIndex == bafferCount.Length || flickCount != bafferCount[flickIndex]) return;


        if (displayCount >= 1) displayCount = displayCount * 2;

            flickIndex++;

        pumpRender.sprite = pumpkinsSp[flickIndex];
        
    }

    /// <summary>
    /// パンプ菌を生成する場所を取得
    /// </summary>
    void CreatePos()
    {
        int nowBomCount = bomCount.NowBomCount();

        //爆弾を生成できるのは、ほかの爆弾がない時 かつ　爆弾の数が０でないときだけ
        if (pumpking.transform.childCount != 0 || nowBomCount == 0 || playerMove.IsTouch != true) return;


        if (Input.GetButtonUp("Fire1"))
        {
            pumpkinParent.SetActive(false);

            //rayの生成
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            
            //rayと衝突していなかったら以降の処理をしない
            if (Physics.Raycast(ray, out hit) == false) return;

            Vector3 createPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);

            minionParent = Instantiate(massParentPre, createPos, Quaternion.identity);

            minionMar = minionParent.GetComponent<MinionManager>();

            //次のボムのタイプを取得
            BomType nextBom = bomCount.NextBomType();

            //パンプキングからパンプ菌を発射
            throwBom.ThrowingBall(createPos,nextBom);

            pumpRender.sprite = pumpkinsSp[0];

            //ボムの数を減らす
            bomCount.UseBom();
        }
    }

    /// <summary>
    /// パンプ菌を作る
    /// </summary>
    /// <param name="parentObj"></param>
    void MinionsCreate(GameObject parentObj)
    {
        Vector3 position = parentObj.transform.position;
        Vector2 size = new Vector2(4.0f, 4.0f);

        camShake.DoShake();

        //攻撃してくるパンプキンを生成
        attackPumpkin = Instantiate(pumpkinPre, position, Quaternion.identity, parentObj.transform);

        StatusSet();

        //エフェクト生成
        Instantiate(createEffect, position, Quaternion.identity,minionParent.transform);


        //見た目だけのパンプキンを生成
        for (int i = 1; i < displayCount; i++)
        {

            float x = Random.Range(position.x - size.x / 2, position.x + size.x / 2);
            float z = Random.Range(position.z - size.y / 2, position.z + size.y / 2);

            Instantiate(displayPumpkinPre, new Vector3(x, position.y, z), Quaternion.identity, parentObj.transform);
        }

        FlickInitialize();
    }

    /// <summary>
    /// 分裂の数に応じて、それぞれのステータスをセットする
    /// </summary>
    void StatusSet()
    {
        PumpAI pumpAI = attackPumpkin.GetComponent<PumpAI>();

        switch (displayCount)
        {
            case 1:
                pumpAI.status = status[0];
                break;

            case 2:
                pumpAI.status = status[1];
                break;

            case 4:
                pumpAI.status = status[2];
                break;

            case 8:
                pumpAI.status = status[3];
                break;
        }

    }
}