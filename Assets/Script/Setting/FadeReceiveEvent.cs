using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeReceiveEvent : MonoBehaviour {

    public bool IsAnimEnd { get; private set; }
    

    public void End()
    {
        Debug.Log("ok");
        IsAnimEnd = true;
    }
}
