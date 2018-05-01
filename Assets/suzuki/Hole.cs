using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField]
    bool invincible;        //無敵
    [SerializeField]
    bool infection;         //感染
    [SerializeField]
    int HP;
    [SerializeField]
    int DecreaseHP;
    [SerializeField]
    float invincibleTime;   //無敵時間
    [SerializeField]
    CapsuleCollider col;    //判定
    [SerializeField]
    Material nextColor;     //変更色(仮)
    Material defaultColor;  //変更色(仮)

    HoleManager manager;    //マネジメントスクリプト

	void Start ()
    {
        //初期値
        invincible = false;
        infection = false;
        HP = 1000;
        DecreaseHP = 50;
        invincibleTime = 0.5f;
        defaultColor = gameObject.GetComponent<Renderer>().material;
        manager = transform.root.GetComponent<HoleManager>();
    }

    /// <summary>
    /// 判定範囲調整
    /// </summary>
    /// <param name="f"></param>
    public void SetColider(float f)
    {
        col = GetComponent<CapsuleCollider>();
        col.radius = f;
        col.height = col.radius * 100 + 22;
        col.center = new Vector3(0, 12 + col.radius * 10, 0);
    }

    public void Flash(float f)
    {
        StartCoroutine(FlashCoroutine(f));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="f"></param>
    public void Invasion(float f)
    {
        infection = true;
        StartCoroutine(Chain(f));
    }

    //void OnTriggerEnter(Collider collider)
    //{
    //    if (b == false && collider.tag == "Player")
    //    {
    //        b = true;

    //        //HoleManager伝える
    //        manager.Unification(gameObject.GetComponent<Hole>());
    //    }
    //}

    void OnTriggerStay(Collider collider)
    {
        if (infection == false && collider.tag == "Player")
        {
            if (invincible == false && HP > 0)
            {
                //耐久値減少
                HP -= DecreaseHP;

                if (HP <= 0)//感染
                {
                    infection = true;

                    //感染をHoleManagerに伝える
                    manager.Unification(gameObject.GetComponent<Hole>(), HP);
                }
                else//通常
                {
                    //無敵時間
                    StartCoroutine(InvincibleTime());

                    //感染をHoleManagerに伝える
                    manager.Unification(gameObject.GetComponent<Hole>(), HP);
                }
            }
        }
    }

    IEnumerator FlashCoroutine(float f)
    {
        yield return new WaitForSeconds(f);
        gameObject.GetComponent<Renderer>().material = nextColor;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<Renderer>().material = defaultColor;
    }

    /// <summary>
    /// 無敵時間(invincibleTime秒ダメージを受けない)
    /// </summary>
    /// <returns></returns>
    IEnumerator InvincibleTime()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibleTime);
        invincible = false;
    }

    IEnumerator Chain(float f)
    {
        yield return new WaitForSeconds(f);

        //色変え(仮)
        gameObject.GetComponent<Renderer>().material = nextColor;
    }
}
