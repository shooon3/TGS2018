using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [Header("振動時間")]
    public float shakeTime;

    [Header("震度")]
    public float magunitude;
    /// <summary>
    /// カメラシェイク処理
    /// </summary>
    public void DoShake()
    {
        StartCoroutine(CamShake(shakeTime,magunitude));
    }

    IEnumerator CamShake(float shakeTime,float magnitude)
    {
        Vector3 pos = transform.position;

        float nowTime = 0;

        while(nowTime < shakeTime)
        {
            float x = pos.x + Random.Range(-1.0f, 1.0f) * magnitude;
            float y = pos.y + Random.Range(-1.0f, 1.0f) * magnitude;

            transform.position = new Vector3(x, y, pos.z);

            nowTime += Time.deltaTime;

            yield return null;
        }


        transform.position = pos;
    }
}
