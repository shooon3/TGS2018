using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    //-------------------------------
    // public
    //-------------------------------


    //-------------------------------
    // private
    //-------------------------------
    Ray ray; //rayの作成
    RaycastHit hit; //rayが衝突したコライダーの情報を得る

    Vector3 mousePos;

    GameObject childObj; //子オブジェクト(吹き出し)

    float move_x;
    float move_z;

   [Header("移動スピードの調整"), Range(1, 20)]
    float speed;



    // Use this for initialization
    void Start () {
        //hit = new RaycastHit();
        childObj = transform.GetChild(0).gameObject;

        //非表示
        childObj.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        RayCreate();
    }

    /// <summary>
    /// rayの生成
    /// </summary>
    void RayCreate()
    {
        if (Input.GetButton("Fire1"))
        {
            Move();

            //rayの作成、タッチしたスクリーン座標をrayに変換
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //押しているときは表示
            childObj.SetActive(true);
        }
        //押していない間は非表示
        else if (Input.GetButtonUp("Fire1")) childObj.SetActive(false);
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
}
