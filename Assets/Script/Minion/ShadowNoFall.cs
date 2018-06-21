using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowNoFall : MonoBehaviour
{
    // ターゲットオブジェクトの Transform を格納する変数
    public Transform target;
    GameObject bombObj;
    // ターゲットオブジェクトの座標からオフセットする値
    public float offsetZ;

    ThrowBom throwBom;

    // Use this for initialization
    void Start()
    {
        throwBom = transform.parent.GetComponent<ThrowBom>();
        bombObj = throwBom.GetBomObj();
    }
    private void LateUpdate()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        target = bombObj.transform;
        // オブジェクトの座標を変数 pos に格納
        Vector3 pos = transform.position;
        // ターゲットオブジェクトのY座標に変数 offset のオフセット値を加えて
        // 変数 posのX座標に代入
        pos.x = target.position.x;
        // 変数 posのZ座標に代入
        pos.z = target.position.z + offsetZ;
        // 変数 pos の値をオブジェクト座標に格納
        transform.position = pos;
    }
}
