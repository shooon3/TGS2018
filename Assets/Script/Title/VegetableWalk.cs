using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VegetableWalk : MonoBehaviour {

    public TitleHoleType enemyType;

    List<TitleTarget> holeLis = new List<TitleTarget>();

    GameObject nearTarget;

    NavMeshAgent agent;

    Animator animator;

    // Use this for initialization
    void Start () {

        TitleTarget[] targetArray = FindObjectsOfType<TitleTarget>();

        foreach (TitleTarget t in targetArray)
        {
            if(t.type == enemyType) holeLis.Add(t);
        }

        agent = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        animator.SetTrigger("IsMove");
    }
	
	// Update is called once per frame
	void Update ()
    {
        SerchTarget();

        AttackRotation();
    }

    void SerchTarget()
    {
        if(nearTarget == null)
        {
            StartCoroutine(Delay());
        }

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);

        int random = Random.Range(0, holeLis.Count);

        nearTarget = holeLis[random].gameObject;

        agent.SetDestination(nearTarget.transform.position);

    }

    void AttackRotation()
    {
        if (nearTarget == null) return;

        Vector3 targetRotate = nearTarget.transform.position - transform.position;

        targetRotate = new Vector3(targetRotate.x, 0, targetRotate.z);

        Quaternion rotation = Quaternion.LookRotation(targetRotate);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Hole")
        {
            nearTarget = null;
        }
    }
}
