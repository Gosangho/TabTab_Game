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
    //public static int swordGirl1KillCount;
    //public static int swordGirl2KillCount;
    //public static int leonKillCount;

    void Start()
    {
        //swordGirl1KillCount = DataManager.Instance.swordGirl1.totalKillScore;
        //swordGirl2KillCount = DataManager.Instance.swordGirl2.totalKillScore;
        //leonKillCount = DataManager.Instance.leon.totalKillScore;
        scoreText = GetComponent<TextMeshProUGUI>();
        score = 0;
    }

    void Update()
    {
        scoreText.text = "Score : " + score.ToString();

        // score가 현재 캐릭터가 가지고있는 베스트스코어보다 크다면 score가 캐릭터의 베스트 스코어로 변경됨
        if (SelectCharacter.swordGirl1 && score > DataManager.Instance.swordGirl1.bestScore)
        {
            DataManager.Instance.swordGirl1.bestScore = score;
            DataManager.Instance.SaveGameData();
        }
        else if (SelectCharacter.swordGirl2 && score > DataManager.Instance.swordGirl2.bestScore)
        {
            DataManager.Instance.swordGirl2.bestScore = score;
            DataManager.Instance.SaveGameData();
        }
        else if (SelectCharacter.leon && score > DataManager.Instance.leon.bestScore)
        {
            DataManager.Instance.leon.bestScore = score;
            DataManager.Instance.SaveGameData();
        }
        else
        {
            return;
        }

        
    }
    
}
