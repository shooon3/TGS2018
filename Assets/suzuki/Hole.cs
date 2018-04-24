using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public int myNumber;    //管理番号
    [SerializeField]
    bool b;                 //
    [SerializeField]
    CapsuleCollider col;    //判定
    [SerializeField]
    Material nextColor;     //変更色(仮)

    HoleManager manager;    //マネジメントスクリプト

	void Start ()
    {
        //初期値
        b = false;
        col = GetComponent<CapsuleCollider>();
        manager = transform.root.GetComponent<HoleManager>();

        //col.center = new Vector3(0, 12, 0);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="f"></param>
    public void Invasion(float f)
    {
        b = true;
        StartCoroutine(Chain(f));
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if (b == false && collider.tag == "Player")
        {
            b = true;

            //HoleManager伝える
            manager.Unification(myNumber);
        }
    }

    IEnumerator Chain(float f)
    {
        yield return new WaitForSeconds(f);

        //色変え(仮)
        gameObject.GetComponent<Renderer>().material = nextColor;
    }
}
