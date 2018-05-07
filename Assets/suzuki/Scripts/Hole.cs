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
    int hp;
    int decreaseHP;
    float flashTime;        //点滅時間
    float invincibleTime;   //無敵時間

    [SerializeField]
    CapsuleCollider col;    //判定
    Material nextColor;     //変更色(仮)
    Material defaultColor;  //変更色(仮)

    HoleManager manager;    //マネジメントスクリプト

	void Start ()
    {
        manager = transform.root.GetComponent<HoleManager>();

        //初期値
        invincible = false;
        infection = false;
        hp = manager.HP;
        decreaseHP = manager.DecreaseHP;
        flashTime = manager.FlashTime;
        invincibleTime = manager.InvincibleTime;
        nextColor = manager.NextColor;
        defaultColor = gameObject.GetComponent<Renderer>().material;
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
    /// 同レーンの穴も感染状態にする
    /// </summary>
    /// <param name="f"></param>
    public void Invasion(float f)
    {
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
            if (invincible == false && hp > 0)
            {
                //耐久値減少
                hp -= decreaseHP;

                if (hp <= 0)//感染
                {
                    infection = true;

                    //感染をHoleManagerに伝える
                    manager.Unification(gameObject.GetComponent<Hole>(), hp);
                }
                else//通常
                {
                    //無敵時間
                    StartCoroutine(InvincibleTime());

                    //感染をHoleManagerに伝える
                    manager.Unification(gameObject.GetComponent<Hole>(), hp);
                }
            }
        }
    }

    /// <summary>
    /// ダメージの可視化
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    IEnumerator FlashCoroutine(float f)
    {
        yield return new WaitForSeconds(f);
        gameObject.GetComponent<Renderer>().material = nextColor;
        yield return new WaitForSeconds(flashTime);
        gameObject.GetComponent<Renderer>().material = defaultColor;
    }

    /// <summary>
    /// 無敵時間
    /// </summary>
    /// <returns></returns>
    IEnumerator InvincibleTime()
    {
        //invincibleTime秒ダメージを受けない
        invincible = true;
        yield return new WaitForSeconds(invincibleTime);
        invincible = false;
    }

    IEnumerator Chain(float f)
    {
        yield return new WaitForSeconds(f);

        //感染を可視化(仮)
        gameObject.GetComponent<Renderer>().material = nextColor;
    }
}
