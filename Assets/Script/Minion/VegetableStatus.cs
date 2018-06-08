using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VegetableStatus : ScriptableObject {

    [Header("HP")]
    public int hp;

    [Header("攻撃力")]
    public int pow;

    [Header("攻撃間隔")]
    public float attackInterval;
}
