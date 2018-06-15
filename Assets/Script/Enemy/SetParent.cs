using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParent : MonoBehaviour {

    public Transform parentTransform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.childCount > 1)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).parent = parentTransform;
            }
        }
	}
}
