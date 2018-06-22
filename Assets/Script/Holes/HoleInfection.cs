using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoleInfection : MonoBehaviour {

    //子供のHoleスクリプトを参照
    List<Hole> holeLis = new List<Hole>();

    List<Transform> childLis = new List<Transform>();


	// Use this for initialization
	void Start () {

		foreach(Transform child in transform)
        {
            childLis.Add(child);
        }

        for (int i = 0; i < childLis.Count; i++)
        {
            foreach (Transform child in childLis[i].transform)
            {
                Hole hole = child.GetComponent<Hole>();

                if (hole != null) holeLis.Add(hole);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
    }

    /// <summary>
    /// すべての穴が感染しているかどうか
    /// </summary>
    /// <returns></returns>
    public bool AllInfection()
    {
        for(int i = 0; i < holeLis.Count; i++)
        {
            if (holeLis[i].Infection == false)
            {
                return false;
            }
        }
        return true;
    }

    public float InfectionCount()
    {
        float temp = 0;

        for (int i = 0; i < holeLis.Count; i++)
        {
            if (holeLis[i].Infection)
            {
                temp++;
            }
        }

        return temp;
    }
}
