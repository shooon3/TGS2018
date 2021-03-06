﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBom : MonoBehaviour {


    [Header("射出するオブジェクトをここに割り当てる")]
    public GameObject throwingObject;

    [Header("ボムに追従する影オブジェクトをここに割り当てる")]
    public GameObject shadowObject;

    [Header("ボム発射位置")]
    public GameObject bullet;

    [Header("射出する角度"), Range(0F, 90F)]
    public float throwingAngle;

    GameObject ball;
    GameObject InsShadow;

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>
    /// ボールを射出する
    /// </summary>
    public void ThrowingBall(Vector3 targetPosition,BomType type)
    {
        if (throwingObject == null) return;

        
        Vector3 transformPos = bullet.transform.position;

        //ノーマルボムの生成
        if (type == BomType.normal)
        {
            // Ballオブジェクトの生成
            ball = Instantiate(throwingObject, transformPos, Quaternion.identity);
            InsShadow = Instantiate(shadowObject);
        }
        //スペシャルボムの生成
        else if(type == BomType.special)
        {
            ball = Instantiate(throwingObject, transformPos, Quaternion.identity);
        }

        //ボムをキングの子オブジェクトで管理
        ball.transform.parent = transform;
        InsShadow.transform.parent = transform;

        // 射出角度
        float angle = throwingAngle;

        // 射出速度を算出
        Vector3 velocity = CalculateVelocity(bullet.transform.position, targetPosition, angle);

        // 射出
        Rigidbody rid = ball.GetComponent<Rigidbody>();
        rid.AddForce(velocity * rid.mass , ForceMode.Impulse);
    }

    /// <summary>
    /// ボムオブジェクトを取得
    /// </summary>
    /// <returns></returns>
    public GameObject GetBomObj()
    {
        if (ball == null) return null;

        return ball;
    }

    ///// <summary>
    ///// 影オブジェクトを取得
    ///// </summary>
    ///// <returns></returns>
    //public GameObject GetShadowObj()
    //{
    //    if (InsShadow == null) return null;

    //    return InsShadow;
    //}

    /// <summary>
    /// 標的に命中する射出速度の計算
    /// </summary>
    /// <param name="pointA">射出開始座標</param>
    /// <param name="pointB">標的の座標</param>
    /// <returns>射出速度</returns>
    Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        // 射出角をラジアンに変換
        float rad = angle * Mathf.PI / 180;

        // 水平方向の距離x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        // 垂直方向の距離y
        float y = pointA.y - pointB.y;

        // 斜方投射の公式を初速度について解く
        float speed = Mathf.Sqrt(-Physics.gravity.y  * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // 条件を満たす初速を算出できなければVector3.zeroを返す
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }
}
