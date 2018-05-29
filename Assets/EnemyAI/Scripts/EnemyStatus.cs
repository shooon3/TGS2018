using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int HP;
    public int Attack;
    private float countTime = 0f;
    private int damage = 0;

    int result;
    internal Potato pc;

    // Use this for initialization
    void Start ()
    {
        // 残りHPの割合
        result = (Attack / HP) * 100;
	}

    // Update is called once per frame
    void Update()
    {
        // HPが0以下で削除
        if (HP <= 0)
        {
            Object.Destroy(gameObject);
        }
    }
  
    public void SetDamage(int damage)
    {
    　　 // ダメージの保持
        this.damage += damage;
        countTime = 0f;
    }
	
}
