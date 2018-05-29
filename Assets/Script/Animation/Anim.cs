﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    public Animator animator;
    public GameObject Black;

    // Use this for initialization
    void Start()
    {
        Black.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Black.SetActive(true);
            animator.SetBool("PauseShot", true);
        }
    }
}
