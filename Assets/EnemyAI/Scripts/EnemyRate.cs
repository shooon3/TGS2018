using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Enemyの出現率を扱うやつ

public class EnemyRate : MonoBehaviour
{
    [SerializeField]
    int Enemy1Incidence;

    [SerializeField]
    int Enemy2Incidence;

    [SerializeField]
    int Enemy3Incidence;

    [SerializeField]
    int Enemy4Incidence;

    string[] EnemyNames = { "じゃがいも", "だいこん", "とうもろこし", "とうがらし" };


    // Use this for initialization
    void Start()
    {
        print(GetRandomEnemyName());
    }

    // Update is called once per frame
    void Update()
    {

    }

    string GetRandomEnemyName()
    {
        var incidenceDistoribution = GetIncidenceDistributionList(Enemy1Incidence, Enemy2Incidence, Enemy3Incidence,Enemy4Incidence);

        int rdm = Random.Range(0, incidenceDistoribution.Count);
        return EnemyNames[incidenceDistoribution[rdm]];
    }

    List<int> GetIncidenceDistributionList(params int[] incidences)
    {
        var incidenceList = new List<int>();

        int gcd = GCD(incidences);

        for (int i = 0, len = incidences.Length; i < len; i++)
        {
            int incidence = incidences[i] / gcd;

            for (int j = 0; j <= incidence; j++)
            {
                incidenceList.Add(i);
            }
        }
        return incidenceList;
    }

    int GCD(int[] numbers)
    {
        return numbers.Aggregate(GCD);
    }

    int GCD(int a, int b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }
}