using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField]
    bool invincible;        //無敵
    //[SerializeField]
    //bool infection;         //感染
    [SerializeField]
    int hp;
    int MaxHp;
    int decreaseHP;
    float flashTime;        //点滅時間
    float invincibleTime;   //無敵時間

    //[SerializeField]
    //SphereCollider col;    //判定
    Material nextColor;     //変更色(仮)
    Material defaultColor;  //変更色(仮)

    HoleManager manager;    //マネジメントスクリプト

    //感染
    public bool Infection { get; private set; }

	void Start ()
    {
        //親ブジェクトのHoleManagerを取得
        manager = transform.parent.GetComponent<HoleManager>();

        //初期値
        invincible = false;
        Infection = false;
        MaxHp = manager.MaxHP;
        hp = MaxHp;
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
    //public void SetColider(float f)
    //{
    //    col = GetComponent<CapsuleCollider>();
    //    col.radius = f;
    //    col.height = col.radius * 100 + 22;
    //    col.center = new Vector3(0, 12 + col.radius * 10, 0);
    //}

    /// <summary>
    /// 被ダメージ・回復を可視化
    /// </summary>
    /// <param name="f"></param>
    /// <param name="b"></param>
    public void Flash(float f, bool b)
    {
        if (b == Infection)
        {
            StartCoroutine(FlashCoroutine(f));
        }
    }

    /// <summary>
    /// 同レーンの穴も感染状態にする
    /// </summary>
    /// <param name="f"></param>
    /// <param name="b"></param>
    public void Invasion(float f, bool b)
    {
        if (b == Infection)
        {
            StartCoroutine(Chain(f));
        }
    }

    ///// <summary>
    ///// 接触判定
    ///// </summary>
    ///// <param name="collider"></param>
    //void OnTriggerStay(Collider collider)
    //{
    //    if (!Infection && collider.tag == "Minion") //パンプ菌
    //    {
    //        if (!invincible && hp > 0)
    //        {
    //            //耐久値減少
    //            hp -= decreaseHP;

    //            if (hp <= 0)//感染
    //            {
    //                //感染をHoleManagerに伝える
    //                manager.Unification(gameObject.GetComponent<Hole>(), hp);
    //            }
    //            else//通常
    //            {
    //                //無敵時間
    //                StartCoroutine(InvincibleTime());

    //                //被ダメージをHoleManagerに伝える
    //                manager.Unification(gameObject.GetComponent<Hole>(), hp);
    //            }
    //        }
    //    }
    //    else if (Infection && collider.tag == "Enemy")  //敵
    //    {
    //        if (!invincible && hp < MaxHp)
    //        {
    //            //耐久値上昇
    //            hp += decreaseHP;

    //            if (hp >= MaxHp)//感染解除
    //            {
    //                //除染完了をHoleManagerに伝える
    //                manager.Unification(gameObject.GetComponent<Hole>(), hp);
    //            }
    //            else//通常
    //            {
    //                //無敵時間
    //                StartCoroutine(InvincibleTime());

    //                //回復をHoleManagerに伝える
    //                manager.Unification(gameObject.GetComponent<Hole>(), hp);
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 感染
    /// </summary>
    public void Infectious()
    {
        if (!Infection)
        {
            if (!invincible && hp > 0)
            {
                //耐久値減少
                hp -= decreaseHP;

                if (hp <= 0)//感染
                {
                    //感染をHoleManagerに伝える
                    manager.Unification(gameObject.GetComponent<Hole>(), hp);
                }
                else//通常
                {
                    //無敵時間
                    StartCoroutine(InvincibleTime());

                    //被ダメージをHoleManagerに伝える
                    manager.Unification(gameObject.GetComponent<Hole>(), hp);
                }
            }
        }
    }

    /// <summary>
    /// 除染
    /// </summary>
    public void Decontamination()
    {
        if (Infection)
        {
            if (!invincible && hp < MaxHp)
            {
                //耐久値上昇
                hp += decreaseHP;

                if (hp >= MaxHp)//感染解除
                {
                    //除染完了をHoleManagerに伝える
                    manager.Unification(gameObject.GetComponent<Hole>(), hp);
                }
                else//通常
                {
                    //無敵時間
                    StartCoroutine(InvincibleTime());

                    //回復をHoleManagerに伝える
                    manager.Unification(gameObject.GetComponent<Hole>(), hp);
                }
            }
        }
    }

    /// <summary>
    /// 被ダメージ・回復の可視化(コルーチン)
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    IEnumerator FlashCoroutine(float f)
    {
        //HoleManagerのflashTime秒だけ色を変える
        //fは時差

        if (!Infection)
        {
            yield return new WaitForSeconds(f);
            gameObject.GetComponent<Renderer>().material = nextColor;
            yield return new WaitForSeconds(flashTime);
            gameObject.GetComponent<Renderer>().material = defaultColor;
        }
        else
        {
            yield return new WaitForSeconds(f);
            gameObject.GetComponent<Renderer>().material = defaultColor;
            yield return new WaitForSeconds(flashTime);
            gameObject.GetComponent<Renderer>().material = nextColor;
        }
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

    /// <summary>
    /// 統一化
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    IEnumerator Chain(float f)
    {
        //fは時差
        if (!Infection)
        {
            Infection = true;
            hp = 0;
            yield return new WaitForSeconds(f);

            //感染を可視化(仮)
            gameObject.GetComponent<Renderer>().material = nextColor;

        }
        else
        {
            Infection = false;
            hp = MaxHp;
            yield return new WaitForSeconds(f);

            //除染を可視化(仮)
            gameObject.GetComponent<Renderer>().material = defaultColor;
        }
    }

    public float time = 1;
    void Update()
    {
        if (Infection==true)
        {
            time -= Time.deltaTime;
            if(time<0)
            {
                time = 0;
                this.tag = ("infHole");
            }
        }
    }
}