using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCountPlus : MonoBehaviour {

    public GameObject displayBomb;

    [Range(0, 30)]
    public float time = 1;

    Hole hole;

    float createTimer;

    bool isCreate = true;

    BomCount bomCount;

    Vector3 pumpPos;

    Vector3 startPos;
    Vector3 endPos;

    GameObject pumpObj;

    float startTime;

    float diff;

    float rate;

    // Use this for initialization
    void Start ()
    {
        hole = GetComponent<Hole>();

        bomCount = FindObjectOfType<BomCount>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hole.Infection) return;

        if (isCreate == false)
        {
            createTimer += Time.deltaTime;

            diff = Time.timeSinceLevelLoad - startTime;

            rate = diff / time;

            pumpObj.transform.position = Vector3.Lerp(startPos, endPos, rate);
        }


        if (isCreate)
        {
            pumpObj = Instantiate(displayBomb, transform.position, Quaternion.identity);
            pumpPos = pumpObj.transform.position;

            startPos = new Vector3(pumpPos.x, -17.0f, pumpPos.z);
            endPos = new Vector3(pumpPos.x, -9.5f, pumpPos.z);

            startTime = Time.timeSinceLevelLoad;

            isCreate = false;
        }
        else if (createTimer >= time)
        {
            Debug.Log("きえた");
            bomCount.AddBom(BomType.normal);
            Destroy(pumpObj);
            createTimer = 0;
            rate = 0;
            isCreate = true;
        }

    }
}
