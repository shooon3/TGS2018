using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoleInfection : MonoBehaviour {

    //子供のHoleスクリプトを参照
    List<Hole> holeLis = new List<Hole>();

    List<Transform> childLis = new List<Transform>();

    public GameObject text;

	// Use this for initialization
	void Start () {
        text.SetActive(false);

		foreach(Transform child in transform)
        {
            childLis.Add(child);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(AllInfection()) text.SetActive(true);
    }

    /// <summary>
    /// すべての穴が感染しているかどうか
    /// </summary>
    /// <returns></returns>
    bool AllInfection()
    {
        for (int i = 0; i < childLis.Count; i++)
        {
            foreach (Transform child in childLis[i].transform)
            {
                Hole hole = child.GetComponent<Hole>();

                if (hole != null) holeLis.Add(hole);
            }
        }

        for(int i = 0; i < holeLis.Count; i++)
        {
            if (holeLis[i].Infection == false) return false;
        }
        return true;
    }
}
