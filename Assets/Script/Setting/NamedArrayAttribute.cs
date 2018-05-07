using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 配列の要素(Element)の名前を変える
/// https://qiita.com/miyumiyu/items/eee6cfbdc6078270450c
/// </summary>
public class NamedArrayAttribute : PropertyAttribute
{
    public readonly string[] names;
    public NamedArrayAttribute(string[] names) { this.names = names; }
}
