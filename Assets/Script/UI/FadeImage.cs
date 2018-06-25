using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FadeType
{
    _idel = 0,
    _fadeout,
    _fadein
}

public class FadeImage : MonoBehaviour {

    Camera cam;

    public FadeType type;

    Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		switch(type)
        {
            case FadeType._idel:
                Idle();
                break;

            case FadeType._fadeout:
                FadeOut();
                break;

            case FadeType._fadein:
                FadeIn();
                break;
        }
	}

    void Idle()
    {
        transform.localScale = new Vector3(0, 0, 0);
        animator.SetTrigger("IsIdle");
        animator.ResetTrigger("IsFadeIn");
        animator.ResetTrigger("IsFadeOut");
    }

    void FadeOut()
    {
        animator.SetTrigger("IsFadeOut");
        animator.ResetTrigger("IsFadeIn");
        animator.ResetTrigger("IsIdle");
    }

    void FadeIn()
    {
        animator.SetTrigger("IsFadeIn");
        animator.ResetTrigger("IsFadeOut");
        animator.ResetTrigger("IsIdle");
    }
}
