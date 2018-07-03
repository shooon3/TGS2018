using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackEnemyCreate : MonoBehaviour {

    public HoleInfection infection;

    public TimeCounter countor;

    public BomCount bombCount;

    public GameObject BossAttackEnemy;

    public Transform parent;

    public CameraRotate camRotate;

    public HoleType type;

    [Header("Maxの出現時間"),Range(10,30)]
    public float createTime = 30;

    List<Transform> createPos = new List<Transform>();

    bool isCreate = false;

    float createCycle = 0;

    float time;
    float gameOverTime;

    float random_x, random_z;

    void Start()
    {
        //子オブジェクト取得
        foreach(Transform pos in transform)
        {
            createPos.Add(pos);
        }
    }

    void Update()
    {
        if (infection.AllInfection() || countor.IsStart == false) return;

        if (camRotate.HoleType != type) return;

        time += Time.deltaTime;

        int minCycle = 3;

        //ゲームオーバー条件を追加
        //ボムの数が０かつ、一つも感染した穴がなかったら
        if (bombCount.NowBomCount() == 0)
        {
            gameOverTime += Time.deltaTime;
        }
        else gameOverTime = 0;

        if (gameOverTime > 10 && infection.IsAllKillVirus())
        {
            createCycle = minCycle;
        }
        else
        {
            createCycle = Mathf.Max(createTime - (bombCount.NowBomCount() * 0.5f), minCycle);
        }

        if (time > createCycle)
        {
            int randomType = Random.Range(0, 2);

            if(randomType == 0)
            {
                random_x = Random.Range(createPos[0].position.x, createPos[1].position.x);
                random_z = Random.Range(createPos[0].position.z, createPos[1].position.z);
            }
            else
            {
                random_x = Random.Range(createPos[2].position.x, createPos[3].position.x);
                random_z = Random.Range(createPos[2].position.z, createPos[3].position.z);
            }

            Vector3 vec = new Vector3(random_x,0, random_z);

            GameObject enemy = Instantiate(BossAttackEnemy, vec, Quaternion.identity);
            enemy.transform.SetParent(parent, true);
            time = 0;
        }

    }
}
