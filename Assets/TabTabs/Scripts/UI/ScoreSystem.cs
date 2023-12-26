using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI ScoreText;
    public int Score;
    public static int BestScore;
    void Start()
    {
        ScoreText = GetComponent<TextMeshProUGUI>();
        Score = 0;
    }

    void Update()
    {
        ScoreText.text = "Score : " + Score.ToString();

        if (BestScore < Score)
        {
            BestScore = Score;
        }
    }
}
