using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour {

    [Header("この畑ででる敵")]
    public GameObject enemyObj;

    [Header("ボム生成時のエフェクト")]
    public GameObject bombCreateEffect;

    [Header("爆弾数プラス時のスプライト")]
    public GameObject plusSprite;

    [Header("感染後に畑にできるカボチャ")]
    public GameObject bombDisplay;

    [Header("この畑ででるボス")]
    public GameObject bossEnemy;

    [Header("パンプキング")]
    public GameObject pumpking;

    [Header("感染した際に畑に出るスプライト")]
    public GameObject infectionImage;

    [Header("感染したときのスプライトの親オブジェクト")]
    public Transform infImgarentPos;

    [Header("カボチャ爆弾ができるまでの時間"),Range(0,60)]
    public float createTime = 30;

    /// <summary>
    ///　この畑ででる艇
    /// </summary>
    public GameObject EnemyObj
    {
        get { return EnemyObj; }
        private set { EnemyObj = value; }
    }

    /// <summary>
    /// ボム生成時のエフェクト
    /// </summary>
    public GameObject BombCreateEffect
    {
        get { return bombCreateEffect; }
        private set { bombCreateEffect = value; }
    }

    /// <summary>
    /// 感染後に畑にできるカボチャ
    /// </summary>
    public GameObject BombDisplay
    {
        get { return bombDisplay; }
        private set { bombDisplay = value; }
    }

    /// <summary>
    /// この畑ででるボス
    /// </summary>
    public GameObject BossEnemy
    {
        get { return bossEnemy; }
        private set { bossEnemy = value; }
    }

    /// <summary>
    /// パンプキング
    /// </summary>
    public GameObject Pumpking
    {
        get { return pumpking; }
        private set { pumpking = value; }
    }

    /// <summary>
    /// 感染した際に畑に出るスプライト
    /// </summary>
    public GameObject InfectionImage
    {
        get { return infectionImage; }
        private set { infectionImage = value; }
    }

    /// <summary>
    /// 感染したときのスプライトの親オブジェクト
    /// </summary>
    public Transform InfImgParent
    {
        get { return infImgarentPos; }
        private set { infImgarentPos = value; }
    }

    /// <summary>
    /// カボチャ爆弾ができるまでの時間
    /// </summary>
    public float CreateTime
    {
        get { return createTime; }
        private set { createTime = value; }
    }
}
