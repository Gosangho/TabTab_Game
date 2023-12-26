using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI scoreText;
    public int score;
    public static int bestScore; // 캐릭터중 최고의 기록을 저장
    public static int swordGirl1BestScore;
    public static int swordGirl2BestScore;
    public static int leonBestScore;
    public static int swordGirl1KillCount;
    public static int swordGirl2KillCount;
    public static int leonKillCount;

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        score = 0;
    }

    void Update()
    {
        scoreText.text = "Score : " + score.ToString();

        if (SelectCharacter.swordGirl1)
        {
            
        }
        else if (SelectCharacter.swordGirl2)
        {

        }
        else
        {

        }
        
    }
}
