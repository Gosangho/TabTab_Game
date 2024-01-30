using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class SelectCharacter : MonoBehaviour
{
    public static bool swordGirl1;
    public static bool swordGirl2;
    public static bool swordGirl3;
    public static bool leon;
    public Image bestScoreCharacterImage;
    public TextMeshProUGUI bestcharacterName;
    public TextMeshProUGUI bestcharacterBestScore;
    public TextMeshProUGUI bestcharacterKillCount;
    public Image swordGirl1Image;
    public Image swordGirl2Image;
    public Image swordGirl3Image;
    public Image leonImage;
    public Sprite swordGirl1Sprite;
    public Sprite swordGirl2Sprite;
    public Sprite swordGirl3Sprite;
    public Sprite leonSprite;
    public TextMeshProUGUI bestScore;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterBestScore;
    public TextMeshProUGUI characterKillCount;
    public TextMeshProUGUI playerGold;
    Dictionary<string, int> characterScores = new Dictionary<string, int>();
    int characterMaxScroe;

    void Start()
    {
        
        characterScores.Add("SwordGirl1", DataManager.Instance.swordGirl1.bestScore);
        characterScores.Add("SwordGirl2", DataManager.Instance.swordGirl2.bestScore);
        characterScores.Add("SwordGirl3", DataManager.Instance.swordGirl3.bestScore);
        characterScores.Add("Leon", DataManager.Instance.leon.bestScore);

        characterMaxScroe = characterScores.Values.Max();

        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            CharacterImageChange();
            playerGold.text = DataManager.Instance.playerData.Gold.ToString();
            
            if (DataManager.Instance.swordGirl1.bestScore == characterMaxScroe)
            {// 스워드걸1의 점수가 가장 높다면
                bestScoreCharacterImage.sprite = swordGirl1Sprite;
                bestcharacterName.text = "캐릭터 :  Sword1"; 
                bestcharacterBestScore.text = "최고기록 : " + DataManager.Instance.swordGirl1.bestScore.ToString();
                bestcharacterKillCount.text = "킬카운트 : " + DataManager.Instance.swordGirl1.totalKillScore.ToString();
            }
            else if (DataManager.Instance.swordGirl2.bestScore == characterMaxScroe)
            {// 스워드걸2의 점수가 가장 높다면
                bestScoreCharacterImage.sprite = swordGirl2Sprite;
                bestcharacterName.text = "캐릭터 :  Sword2";
                bestcharacterBestScore.text = "최고기록 : " + DataManager.Instance.swordGirl2.bestScore.ToString();
                bestcharacterKillCount.text = "킬카운트 : " + DataManager.Instance.swordGirl2.totalKillScore.ToString();
            }
            else if (DataManager.Instance.leon.bestScore == characterMaxScroe)
            {
                bestScoreCharacterImage.sprite = leonSprite;
                bestcharacterName.text = "캐릭터 :  Leon";
                bestcharacterBestScore.text = "최고기록 : " + DataManager.Instance.leon.bestScore.ToString();
                bestcharacterKillCount.text = "킬카운트 : " + DataManager.Instance.leon.totalKillScore.ToString();
            }
            else if (DataManager.Instance.swordGirl3.bestScore == characterMaxScroe)
            {
                bestScoreCharacterImage.sprite = swordGirl3Sprite;
                bestcharacterName.text = "캐릭터 :  Sword3";
                bestcharacterBestScore.text = "최고기록 : " + DataManager.Instance.swordGirl3.bestScore.ToString();
                bestcharacterKillCount.text = "킬카운트 : " + DataManager.Instance.swordGirl3.totalKillScore.ToString();
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
        swordGirl3 = false;
        leon = false;
        //UpdateImage(swordGirl1Image);
        characterName.text = "캐릭터 :  Sword1";
        characterBestScore.text = "최고기록 : " + DataManager.Instance.swordGirl1.bestScore.ToString();
        characterKillCount.text = "킬카운트 : " + DataManager.Instance.swordGirl1.totalKillScore.ToString();
    }

    public void SelectSwordGirl2()
    {
        if (DataManager.Instance.playerData.SwordGirl2Get)
        { // 스워드걸2를 보유하고 있다면
            swordGirl1 = false;
            swordGirl2 = true;
            swordGirl3 = false;
            leon = false;
        }
        //UpdateImage(swordGirl2Image);
        characterName.text = "캐릭터 :  Sword2";
        characterBestScore.text = "최고기록 : " + DataManager.Instance.swordGirl2.bestScore.ToString();
        characterKillCount.text = "킬카운트 : " + DataManager.Instance.swordGirl2.totalKillScore.ToString();
    }

    public void SelectSwordGirl3()
    {
        if (DataManager.Instance.playerData.SwordGirl3Get)
        {
            swordGirl1 = false;
            swordGirl2 = false;
            swordGirl3 = true;
            leon = false;
        }
        //UpdateImage(swordGirl2Image);
        characterName.text = "캐릭터 :  Sword3";
        characterBestScore.text = "최고기록 : " + DataManager.Instance.swordGirl3.bestScore.ToString();
        characterKillCount.text = "킬카운트 : " + DataManager.Instance.swordGirl3.totalKillScore.ToString();
    }

    public void SelectLeon()
    {
        if (DataManager.Instance.playerData.LeonGet)
        {
            swordGirl1 = false;
            swordGirl2 = false;
            swordGirl3 = false;
            leon = true;
        }
        //UpdateImage(leonImage);
        characterName.text = "캐릭터 :  Leon";
        characterBestScore.text = "최고기록 : " + DataManager.Instance.leon.bestScore.ToString();
        characterKillCount.text = "킬카운트 : " + DataManager.Instance.leon.totalKillScore.ToString();
    }

    void CharacterImageChange()
    {
        if (DataManager.Instance.playerData.SwordGirl2Get)
        {
            swordGirl2Image.sprite = swordGirl2Sprite;
        }

        if (DataManager.Instance.playerData.SwordGirl3Get)
        {
            swordGirl3Image.sprite = swordGirl3Sprite;
        }

        if (DataManager.Instance.playerData.LeonGet)
        {
            leonImage.sprite = leonSprite;
        }
    }
}
