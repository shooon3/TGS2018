using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomMove : MonoBehaviour {

    List<GameObject> pumpkinBom = new List<GameObject>();

    int bomNumber;

    public int max_bomNum;

    public GameObject bomPre; //bomプレファブ

    //--------------------------------------------
    // プロパティ
    //--------------------------------------------
    public int BomNumber
    {
        get { return bomNumber; }
        set
        {
            if (bomNumber >= 0 && bomNumber <= max_bomNum) bomNumber = value;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// ボムの数を増やす
    /// </summary>
    public void AddBom()
    {
        BomNumber++;
    }

    /// <summary>
    /// ボムの数を減らす
    /// </summary>
    public void ReduceBom()
    {
        BomNumber--;
    }
}
