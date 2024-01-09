using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI scoreText;
    public int score;
    public static int bestScore; // ĳ������ �ְ��� ����� ����
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

        // score�� ���� ĳ���Ͱ� �������ִ� ����Ʈ���ھ�� ũ�ٸ� score�� ĳ������ ����Ʈ ���ھ�� �����
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
