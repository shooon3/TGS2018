using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleVegetable : MonoBehaviour {

    Hole hole;

    CreateManager createMgr;

    List<GameObject> spriteImgLis = new List<GameObject>();

    //感染したあとかどうか(一度感染しているかどうか)
    bool isInfectionAfter = false;

    float spriteRange = 15.0f;

    float count = 0.4f;

    // Use this for initialization
    void Start () {

        hole = GetComponent<Hole>();

        createMgr = transform.parent.parent.GetComponent<CreateManager>();

        GameObject sprite = createMgr.InfectionImage;
        Transform spriteParent = createMgr.infImgarentPos;

        for (int i = 0; i < 15; i++)
        {
            float x = Random.Range(transform.position.x - spriteRange, transform.position.x + spriteRange);
            float y = Random.Range(transform.position.z - spriteRange, transform.position.z + spriteRange);

            Vector3 vec = new Vector3(-x, -y, sprite.transform.position.z);

            spriteImgLis.Add(Instantiate(sprite, vec, sprite.transform.rotation));
            spriteImgLis[i].transform.SetParent(spriteParent, false);

            spriteImgLis[i].SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        StartCoroutine(DestroyVegetable());
    }

    IEnumerator DestroyVegetable()
    {
        if (hole.Infection && isInfectionAfter == false)
        {
            if (transform.childCount != 0) Destroy(transform.GetChild(0).gameObject);

            for (int i = 0; i < spriteImgLis.Count; i++)
            {
                spriteImgLis[i].SetActive(true);

                yield return new WaitForSeconds(count);

                count -= i * 0.1f;
                count = Mathf.Max(count, 0.01f);
            }

            isInfectionAfter = true;
        }
        else if (hole.Infection == false && isInfectionAfter == true)
        {
            //if (transform.childCount != 0) Destroy(transform.GetChild(0).gameObject);

            foreach (GameObject obj in spriteImgLis)
            {
                yield return new WaitForSeconds(0.1f);
                obj.SetActive(false);
            }

            isInfectionAfter = false;
        }
    }
}
