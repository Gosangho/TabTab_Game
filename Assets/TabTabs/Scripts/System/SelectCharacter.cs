using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    public static bool swordGirl1 = false;
    public static bool swordGirl2 = false;
    public static bool leon = true;
    public Image bestScoreCharacterImage;
    public TextMeshProUGUI bestcharacterName;
    public TextMeshProUGUI bestcharacterBestScore;
    public TextMeshProUGUI bestcharacterKillCount;
    public Sprite swordGirl1Image;
    public Sprite swordGirl2Image;
    public Sprite leonImage;
    public TextMeshProUGUI bestScore;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterBestScore;
    public TextMeshProUGUI characterKillCount;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            if (ScoreSystem.swordGirl1BestScore > ScoreSystem.swordGirl2BestScore &&
            ScoreSystem.swordGirl1BestScore > ScoreSystem.leonBestScore)
            {// 스워드걸1의 점수가 가장 높다면
                bestScoreCharacterImage.sprite = swordGirl1Image;
                bestcharacterName.text = "캐릭터 : sword1";
                bestcharacterBestScore.text = "최고기록 : " + ScoreSystem.swordGirl1BestScore.ToString();
                bestcharacterKillCount.text = "킬카운트 : " + ScoreSystem.swordGirl1KillCount.ToString();
            }
            else if (ScoreSystem.swordGirl2BestScore > ScoreSystem.swordGirl1BestScore &&
                ScoreSystem.swordGirl2BestScore > ScoreSystem.leonBestScore)
            {// 스워드걸2의 점수가 가장 높다면
                bestScoreCharacterImage.sprite = swordGirl2Image;
                bestcharacterName.text = "캐릭터 : sword2";
                bestcharacterBestScore.text = "최고기록 : " + ScoreSystem.swordGirl2BestScore.ToString();
                bestcharacterKillCount.text = "킬카운트 : " + ScoreSystem.swordGirl2KillCount.ToString();
            }
            else
            {// 레온의 점수가 가장 높다면
                bestScoreCharacterImage.sprite = leonImage;
                bestcharacterName.text = "캐릭터 : leon";
                bestcharacterBestScore.text = "최고기록 : " + ScoreSystem.leonBestScore.ToString();
                bestcharacterKillCount.text = "킬카운트 : " + ScoreSystem.leonKillCount.ToString();
            }
        }
    }

    //public void UpdateImage(Sprite newSprite)
    //{
    //    selectImage.sprite = newSprite;
    //}

    public void SelectSwordGirl1()
    {
        swordGirl1 = true;
        swordGirl2 = false;
        leon = false;
        //UpdateImage(swordGirl1Image);
        characterName.text = "캐릭터 : sword1";
        characterBestScore.text = "최고기록 : " + ScoreSystem.swordGirl1BestScore.ToString();
        characterKillCount.text = "킬카운트 : " + ScoreSystem.swordGirl1KillCount.ToString();
    }

    public void SelectSwordGirl2()
    {
        swordGirl1 = false;
        swordGirl2 = true;
        leon = false;
        //UpdateImage(swordGirl2Image);
        characterName.text = "캐릭터 : sword2";
        characterBestScore.text = "최고기록 : " + ScoreSystem.swordGirl2BestScore.ToString();
        characterKillCount.text = "킬카운트 : " + ScoreSystem.swordGirl2KillCount.ToString();
    }

    public void SelectLeon()
    {
        swordGirl1 = false;
        swordGirl2 = false;
        leon = true;
        //UpdateImage(leonImage);
        characterName.text = "캐릭터 : leon";
        characterBestScore.text = "최고기록 : " + ScoreSystem.leonBestScore.ToString();
        characterKillCount.text = "킬카운트 : " + ScoreSystem.leonKillCount.ToString();
    }
    
}
