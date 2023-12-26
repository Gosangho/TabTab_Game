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
            {// �������1�� ������ ���� ���ٸ�
                bestScoreCharacterImage.sprite = swordGirl1Image;
                bestcharacterName.text = "ĳ���� : sword1";
                bestcharacterBestScore.text = "�ְ��� : " + ScoreSystem.swordGirl1BestScore.ToString();
                bestcharacterKillCount.text = "ųī��Ʈ : " + ScoreSystem.swordGirl1KillCount.ToString();
            }
            else if (ScoreSystem.swordGirl2BestScore > ScoreSystem.swordGirl1BestScore &&
                ScoreSystem.swordGirl2BestScore > ScoreSystem.leonBestScore)
            {// �������2�� ������ ���� ���ٸ�
                bestScoreCharacterImage.sprite = swordGirl2Image;
                bestcharacterName.text = "ĳ���� : sword2";
                bestcharacterBestScore.text = "�ְ��� : " + ScoreSystem.swordGirl2BestScore.ToString();
                bestcharacterKillCount.text = "ųī��Ʈ : " + ScoreSystem.swordGirl2KillCount.ToString();
            }
            else
            {// ������ ������ ���� ���ٸ�
                bestScoreCharacterImage.sprite = leonImage;
                bestcharacterName.text = "ĳ���� : leon";
                bestcharacterBestScore.text = "�ְ��� : " + ScoreSystem.leonBestScore.ToString();
                bestcharacterKillCount.text = "ųī��Ʈ : " + ScoreSystem.leonKillCount.ToString();
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
        characterName.text = "ĳ���� : sword1";
        characterBestScore.text = "�ְ��� : " + ScoreSystem.swordGirl1BestScore.ToString();
        characterKillCount.text = "ųī��Ʈ : " + ScoreSystem.swordGirl1KillCount.ToString();
    }

    public void SelectSwordGirl2()
    {
        swordGirl1 = false;
        swordGirl2 = true;
        leon = false;
        //UpdateImage(swordGirl2Image);
        characterName.text = "ĳ���� : sword2";
        characterBestScore.text = "�ְ��� : " + ScoreSystem.swordGirl2BestScore.ToString();
        characterKillCount.text = "ųī��Ʈ : " + ScoreSystem.swordGirl2KillCount.ToString();
    }

    public void SelectLeon()
    {
        swordGirl1 = false;
        swordGirl2 = false;
        leon = true;
        //UpdateImage(leonImage);
        characterName.text = "ĳ���� : leon";
        characterBestScore.text = "�ְ��� : " + ScoreSystem.leonBestScore.ToString();
        characterKillCount.text = "ųī��Ʈ : " + ScoreSystem.leonKillCount.ToString();
    }
    
}
