using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackEnemyCreate : MonoBehaviour {

    public HoleInfection infection;

    public TimeCounter countor;

    public GameObject BossAttackEnemy;

    bool isCreate = false;

    float time;

    void Start()
    {

    }

    void Update()
    {
        if (infection.AllInfection() || countor.IsStart == false) return;


        //StartCoroutine(WaitTime());

        //if(isCreate)
        //{
        //    isCreate = false;
        //}

        time += Time.deltaTime;

        if(time > 30)
        {
            Instantiate(BossAttackEnemy, transform.position, Quaternion.identity);
            time = 0;
        }

    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(10.0f);

        Instantiate(BossAttackEnemy, transform.position, Quaternion.identity);
    }
}
