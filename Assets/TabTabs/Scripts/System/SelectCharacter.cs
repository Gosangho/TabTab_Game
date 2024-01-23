using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    public static bool swordGirl1;
    public static bool swordGirl2;
    public static bool leon;
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
    public TextMeshProUGUI playerGold;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            playerGold.text = DataManager.Instance.playerData.Gold.ToString();
            
            if (DataManager.Instance.swordGirl1.bestScore >= DataManager.Instance.swordGirl2.bestScore &&
            DataManager.Instance.swordGirl1.bestScore >= DataManager.Instance.leon.bestScore)
            {// �������1�� ������ ���� ���ٸ�
                bestScoreCharacterImage.sprite = swordGirl1Image;
                bestcharacterName.text = "ĳ���� :  Sword1"; 
                bestcharacterBestScore.text = "�ְ��� : " + DataManager.Instance.swordGirl1.bestScore.ToString();
                bestcharacterKillCount.text = "ųī��Ʈ : " + DataManager.Instance.swordGirl1.totalKillScore.ToString();
            }
            else if (DataManager.Instance.swordGirl2.bestScore >= DataManager.Instance.swordGirl1.bestScore &&
                DataManager.Instance.swordGirl2.bestScore >= DataManager.Instance.leon.bestScore)
            {// �������2�� ������ ���� ���ٸ�
                bestScoreCharacterImage.sprite = swordGirl2Image;
                bestcharacterName.text = "ĳ���� :  Sword2";
                bestcharacterBestScore.text = "�ְ��� : " + DataManager.Instance.swordGirl2.bestScore.ToString();
                bestcharacterKillCount.text = "ųī��Ʈ : " + DataManager.Instance.swordGirl2.totalKillScore.ToString();
            }
            else if (DataManager.Instance.leon.bestScore >= DataManager.Instance.swordGirl1.bestScore &&
                DataManager.Instance.leon.bestScore >= DataManager.Instance.swordGirl2.bestScore)
            {
                bestScoreCharacterImage.sprite = leonImage;
                bestcharacterName.text = "ĳ���� :  Leon";
                bestcharacterBestScore.text = "�ְ��� : " + DataManager.Instance.leon.bestScore.ToString();
                bestcharacterKillCount.text = "ųī��Ʈ : " + DataManager.Instance.leon.totalKillScore.ToString();
            }
            else
            {
                return;
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
        characterName.text = "ĳ���� :  Sword1";
        characterBestScore.text = "�ְ��� : " + DataManager.Instance.swordGirl1.bestScore.ToString();
        characterKillCount.text = "ųī��Ʈ : " + DataManager.Instance.swordGirl1.totalKillScore.ToString();
    }

    public void SelectSwordGirl2()
    {
        swordGirl1 = false;
        swordGirl2 = true;
        leon = false;
        //UpdateImage(swordGirl2Image);
        characterName.text = "ĳ���� :  Sword2";
        characterBestScore.text = "�ְ��� : " + DataManager.Instance.swordGirl2.bestScore.ToString();
        characterKillCount.text = "ųī��Ʈ : " + DataManager.Instance.swordGirl2.totalKillScore.ToString();
    }

    public void SelectLeon()
    {
        swordGirl1 = false;
        swordGirl2 = false;
        leon = true;
        //UpdateImage(leonImage);
        characterName.text = "ĳ���� :  Leon";
        characterBestScore.text = "�ְ��� : " + DataManager.Instance.leon.bestScore.ToString();
        characterKillCount.text = "ųī��Ʈ : " + DataManager.Instance.leon.totalKillScore.ToString();
    }
    
}
