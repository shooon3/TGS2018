﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materials : MonoBehaviour
{
    //public Vector3 MainFlow;
    //public Vector3 DetaiFlow;
    //public Material Test1;
    //public Material Test2;
    //public Material Test3;

     public bool test;

    public Material[] mats;
    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        renderer.material = mats[0];

    }
}
