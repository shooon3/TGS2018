using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// なんでもインスペクタに表示していくやつ
/// http://klabgames.tech.blog.jp.klab.com/archives/1047665593.html
/// </summary>
public class DebuggingInspector : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
