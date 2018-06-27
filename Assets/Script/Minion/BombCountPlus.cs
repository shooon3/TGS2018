using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCountPlus : MonoBehaviour {

    Hole hole;

    CreateManager createMgr;

    bool isCreate = true;

    BomCount bomCount;

    Vector3 pumpPos;
    Vector3 startPos;
    Vector3 endPos;

    GameObject pumpObj;
    GameObject plusEffect;

    float startTime;
    float createTimer;
    float diff;
    float rate;

    // Use this for initialization
    void Start ()
    {
        hole = GetComponent<Hole>();

        bomCount = FindObjectOfType<BomCount>();

        createMgr = transform.parent.parent.GetComponent<CreateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hole.Infection) return;

        if (isCreate)
        {
            pumpObj = Instantiate(createMgr.BombDisplay, transform.position, createMgr.BombDisplay.transform.rotation);

            pumpPos = pumpObj.transform.position;

            startPos = new Vector3(pumpPos.x, -17.0f, pumpPos.z);
            endPos = new Vector3(pumpPos.x, -12.5f, pumpPos.z);

            startTime = Time.timeSinceLevelLoad;

            isCreate = false;
        }
        else if (isCreate == false)
        {
            createTimer += Time.deltaTime;

            diff = Time.timeSinceLevelLoad - startTime;

            rate = diff / createMgr.CreateTime;

            pumpObj.transform.position = Vector3.Lerp(startPos, endPos, rate);
        }

        if (createTimer >= createMgr.CreateTime)
        {
            bomCount.AddBom(BomType.normal);

            Vector3 offset = new Vector3(transform.position.x, createMgr.bombCreateEffect.transform.position.y, transform.position.z);

            GameObject effect = Instantiate(createMgr.bombCreateEffect, offset, Quaternion.identity);

            Destroy(effect,3.0f);
            Destroy(pumpObj,1.0f);

            createTimer = 0;
            rate = 0;
            isCreate = true;
        }
    }

    void OnBecameInvisible()
    {

        enabled = false;

    }
    void OnBecameVisible()
    {

        enabled = true;

    }

}
