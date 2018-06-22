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

struct PumpCreateData
{
    public int displayCount;
    public BomManager bomMar;
    public GameObject parentObj;
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

    public GameObject bossPumpkin;

    [Header("追従するだけのパンプ菌")]
    public GameObject displayPumpkinPre;

    public GameObject bossDisp;

    [Header("パンプ菌集団の親オブジェクト")]
    public GameObject massParentPre;

    [Header("パンプキング")]
    public GameObject pumpking;

    [Header("ボムの最初の数")]
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

    BomManager bomMar;
    ThrowBom throwBom;
    BomCount bomCount;
    PlayerMove playerMove;
    BaseBossEnemy boss;

    float touchNowPosX; //現在のタッチポジション
    float startFlickX; //タッチした直後のポジション(タッチ直後にフリック判定にならないようにするための除外用変数)
    float memoryPos; //1フレーム前の位置を記憶する

    int flickCount; //フリックした回数
    int displayCount; //パンプキンを出す数
    int beforeCount;
    int flickIndex;

    int layerMask;

    bool isBossAttack;
    bool delay = true;

    FlickState flickState; //フリックされた方向
    FlickState nextFlickState; //次にフリックする方向

    List<PumpCreateData> dataLis = new List<PumpCreateData>();

    //-------------------------------------
    // 関数
    //-------------------------------------

    // Use this for initialization
    void Start () {

        pumpkinParent = transform.GetChild(0).gameObject;

        pumpkinParent.SetActive(false);

        throwBom = pumpking.GetComponent<ThrowBom>();
        bomCount = GetComponent<BomCount>();
        playerMove = GetComponent<PlayerMove>();

        FlickInitialize();

        layerMask = LayerMask.GetMask(new string[] { "Stage", "Boss" });
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMove.IsStart != true || playerMove.IsClear != false) return;

        GetTouchPos();
        Flick();

        if (delay == true)
        {
            CreatePos();
            DisplayPampking();

            StartCoroutine(Delay());
        }
        if(delay == false)
        {
            pumpRender.sprite = pumpkinsSp[4];
        }

        if (dataLis.Count < 0) return;

        for(int i = 0; i < dataLis.Count; i++)
        {
            //パンプ菌が生成できるようになったら(爆弾が地面に衝突してたら)
            if (dataLis[i].bomMar != null && dataLis[i].bomMar.IsCollision)
            {
                MinionsCreate(dataLis[i].parentObj,dataLis[i].displayCount,dataLis[i].bomMar.IsAttackBoss,dataLis[i].bomMar.ColBossObj);

                dataLis.Remove(dataLis[i]);
                continue;
            }
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

        pumpRender.sprite = pumpkinsSp[0];

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
        if (nowBomCount == 0 || playerMove.IsTouch != true) return;
        

        if (Input.GetButtonUp("Fire1"))
        {
            pumpkinParent.SetActive(false);

            //rayの生成
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            //rayと衝突していなかったら以降の処理をしない
            if (Physics.Raycast(ray, out hit,layerMask) == false)
            {
                FlickInitialize();
                return;
            }

            Vector3 createPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);

            minionParent = Instantiate(massParentPre, createPos, Quaternion.identity);

            //次のボムのタイプを取得
            BomType nextBom = bomCount.NextBomType();

            //パンプキングからパンプ菌を発射
            throwBom.ThrowingBall(createPos,nextBom);

            bomMar = throwBom.GetBomObj().GetComponent<BomManager>();

            PumpCreateData pumpData;

            pumpData.bomMar = bomMar;
            pumpData.displayCount = displayCount;
            pumpData.parentObj = minionParent;

            dataLis.Add(pumpData);

            //ボムの数を減らす
            bomCount.UseBom();

            FlickInitialize();

            delay = false;
        }
    }

    /// <summary>
    /// パンプ菌を作る
    /// </summary>
    /// <param name="parentObj"></param>
    void MinionsCreate(GameObject parentObj,int createPumpkin,bool isBossAttack,GameObject bossObj)
    {
        Vector3 position = parentObj.transform.position;
        Vector2 size = new Vector2(4.0f, 4.0f);

        //揺らす
        camShake.DoShake(0.25f,0.5f);
        //攻撃してくるパンプキンを生成
        if (isBossAttack)
        {
            attackPumpkin = Instantiate(bossPumpkin, position, Quaternion.identity, parentObj.transform);

            parentObj.transform.parent = bossObj.transform;
        }
        else
        {
            attackPumpkin = Instantiate(pumpkinPre, position, Quaternion.identity, parentObj.transform);
        }
        StatusSet(createPumpkin);

        Vector3 effectOffset = new Vector3(position.x, position.y + 4.0f, position.z);

        //エフェクト生成
        Instantiate(createEffect, effectOffset, Quaternion.identity,parentObj.transform);

        Vector3 vec = Vector3.zero;

        float x = 0, y = 0, z = 0;

        //見た目だけのパンプキンを生成
        for (int i = 1; i < createPumpkin; i++)
        {

            if (isBossAttack)
            {
                x = Random.Range(position.x - size.x / 2, position.x + size.x / 2);
                y = Random.Range(position.y - size.y / 2, position.y + size.y / 2);


                vec = new Vector3(x, y, position.z);

                Instantiate(bossDisp, vec, Quaternion.identity, parentObj.transform);

            }
            else
            {
                x = Random.Range(position.x - size.x / 2, position.x + size.x / 2);
                z = Random.Range(position.z - size.y / 2, position.z + size.y / 2);

                vec = new Vector3(x, -9.5f, z);

                Instantiate(displayPumpkinPre, vec, Quaternion.identity, parentObj.transform);
            }
        }
        //FlickInitialize();
    }

    /// <summary>
    /// 分裂の数に応じて、それぞれのステータスをセットする
    /// </summary>
    void StatusSet(int displayCount)
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

    IEnumerator DelaySpriteReset()
    {
        yield return new WaitForSeconds(0.5f);
        pumpRender.sprite = pumpkinsSp[0];
    }

    IEnumerator Delay()
    {
        if (delay == false)
        {
            yield return new WaitForSeconds(0.2f);
            delay = true;
        }

        yield return null;
    }
}