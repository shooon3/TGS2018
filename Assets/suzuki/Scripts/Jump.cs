using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField]
    float jump;
    
    [SerializeField]
    Rigidbody rb;
    float ground;
    bool jumping;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        ground = -9;
        jumping = false;
    }

    void Update ()
    {
        if (!jumping)
        {
            if (transform.position.y < ground)
            {
                StartCoroutine(Span());
            }
        }
	}

    void Jumping()
    {
        rb.AddForce(Vector2.up * jump);
        jumping = false;
    }

    IEnumerator Span()
    {
        jumping = true;
        rb.AddForce(Vector2.down * jump);
        yield return new WaitForSeconds(0.5f);
        Jumping();
    }
}
