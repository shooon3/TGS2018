using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopEffe : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    public GameObject c;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Pop);
        a.SetActive(false);
        b.SetActive(false);
        c.SetActive(false);
    }
	
    public IEnumerator Pop()
    {
        yield return new WaitForSeconds(0.5f);
        a.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        b.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        c.SetActive(false);
        yield break;

    }

	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Push");
        }
	}
}
