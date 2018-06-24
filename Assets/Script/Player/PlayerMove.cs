using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour {

    //-------------------------------
    // public
    //-------------------------------

    [Header("タイムカウンター")]
    public TimeCounter timeCount;

    //-------------------------------
    // private
    //-------------------------------
    Ray ray; //rayの作成
    RaycastHit hit; //rayが衝突したコライダーの情報を得る

    Vector3 mousePos;

    //GameObject childObj; //子オブジェクト(吹き出し)

    float move_x;
    float move_z;

   [Header("移動スピードの調整"), Range(1, 20)]
    float speed;


    /// <summary>
    /// UI以外のところをタッチしているかどうか
    /// </summary>
    public bool IsTouch { get; private set; }

    public bool IsStart { get; private set; }

    public bool IsClear { get; private set; }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        IsStart = timeCount.IsStart;
        IsClear = timeCount.IsClear;

        GameObject front = Camera.main.gameObject;
        transform.LookAt(front.transform.position);

        if (IsStart != true || IsClear != false) return;

        RayCreate();
    }

    /// <summary>
    /// rayの生成
    /// </summary>
    void RayCreate()
    {

        //タッチしたところがUIの上じゃなかったら判定しない
#if UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject())
        {
            IsTouch = false;
            return;
        }
#else
        for (int i = 0; i < Input.touchCount; ++i)
        {
            Touch touch = Input.touches[i];
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                IsTouch = false;
                return;

            }
        }
#endif

        if (Input.GetButton("Fire1"))
        {
            Move();

            //rayの作成、タッチしたスクリーン座標をrayに変換
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            IsTouch = true;
        }
    }

    /// <summary>
    /// タッチした位置がプレイヤーの位置かどうか
    /// </summary>
    /// <returns>プレイヤーの位置だった場合true</returns>
    bool PlayerCollider()
    {
        //rayがオブジェクトに当たっていなかったらfalse
        if (Physics.Raycast(ray, out hit) == false) return false;

        //衝突したオブジェクトがプレイヤーだったらtrue
        if(hit.collider.gameObject == gameObject) return true;
        else return false;
    }

    /// <summary>
    /// プレイヤーの移動
    /// </summary>
    void Move()
    {
        Vector3 touchScreenPosition = Input.mousePosition;

        touchScreenPosition.x = Mathf.Clamp(Input.mousePosition.x, 0, Screen.width);
        touchScreenPosition.y = Mathf.Clamp(Input.mousePosition.y, 0, Screen.height);

        // カメラから離す
        touchScreenPosition.z = 20.0f;

        Camera gameCamera = Camera.main;
        Vector3 touchWorldPosition = gameCamera.ScreenToWorldPoint(touchScreenPosition);
        
        transform.position = touchWorldPosition;
    }

    bool Interver()
    {
        int count = 0;

        while(count > 1.0f)
        {
            count++;
        }

        if (count <= 1.0f) return false;
        else return true;
    }
}
