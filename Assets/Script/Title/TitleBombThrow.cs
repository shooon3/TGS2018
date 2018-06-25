using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBombThrow : MonoBehaviour
{
    ThrowBom throwBom;

    CameraShake camShake;

    GameObject bomb;

    bool isFirst = true;

    // Use this for initialization
    void Start()
    {
        throwBom = GetComponent<ThrowBom>();

        camShake = Camera.main.GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //rayの生成
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            //rayと衝突していなかったら以降の処理をしない
            if (Physics.Raycast(ray, out hit) && isFirst)
            {
                Vector3 vec = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                //パンプキングからパンプ菌を発射
                throwBom.ThrowingBall(vec, BomType.normal);

                bomb = throwBom.GetBomObj();
                isFirst = false;

            }


        }
        if (bomb == null && isFirst == false)
        {
            camShake.DoShake(0.5f, 0.5f);
            isFirst = true;
        }
    }
}
