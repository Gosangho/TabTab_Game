using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ranking : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankingUi;
    [SerializeField] private int myRankingScore;
    void Update()
    {
        myRankingScore = ScoreSystem.BestScore;
        rankingUi.text = "ID  TTW    " + myRankingScore;
    }
}
