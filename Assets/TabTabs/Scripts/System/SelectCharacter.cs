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
    public GameObject swordGirl3Purchase;
    public GameObject leonPurchase;
    public TextMeshProUGUI goldLackText;
    public TextMeshProUGUI goldLackText1;
    public TextMeshProUGUI swordGirl3PurchaseText;
    public TextMeshProUGUI leonPurchaseText;
    public GameObject purchaseSuccess;

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
            {// �������1�� ������ ���� ���ٸ�
                bestScoreCharacterImage.sprite = swordGirl1Sprite;
                bestcharacterName.text = "ĳ���� :  Sword1"; 
                bestcharacterBestScore.text = "�ְ���� : " + DataManager.Instance.swordGirl1.bestScore.ToString();
                bestcharacterKillCount.text = "ųī��Ʈ : " + DataManager.Instance.swordGirl1.totalKillScore.ToString();
            }
            else if (DataManager.Instance.swordGirl2.bestScore == characterMaxScroe)
            {// �������2�� ������ ���� ���ٸ�
                bestScoreCharacterImage.sprite = swordGirl2Sprite;
                bestcharacterName.text = "ĳ���� :  Sword2";
                bestcharacterBestScore.text = "�ְ���� : " + DataManager.Instance.swordGirl2.bestScore.ToString();
                bestcharacterKillCount.text = "ųī��Ʈ : " + DataManager.Instance.swordGirl2.totalKillScore.ToString();
            }
            else if (DataManager.Instance.leon.bestScore == characterMaxScroe)
            {
                bestScoreCharacterImage.sprite = leonSprite;
                bestcharacterName.text = "ĳ���� :  Leon";
                bestcharacterBestScore.text = "�ְ���� : " + DataManager.Instance.leon.bestScore.ToString();
                bestcharacterKillCount.text = "ųī��Ʈ : " + DataManager.Instance.leon.totalKillScore.ToString();
            }
            else if (DataManager.Instance.swordGirl3.bestScore == characterMaxScroe)
            {
                bestScoreCharacterImage.sprite = swordGirl3Sprite;
                bestcharacterName.text = "ĳ���� :  Sword3";
                bestcharacterBestScore.text = "�ְ���� : " + DataManager.Instance.swordGirl3.bestScore.ToString();
                bestcharacterKillCount.text = "ųī��Ʈ : " + DataManager.Instance.swordGirl3.totalKillScore.ToString();
            }
            else
            {
                return;
            }
        }
    }

    public void SelectSwordGirl1()
    {
        swordGirl1 = true;
        swordGirl2 = false;
        swordGirl3 = false;
        leon = false;
        //UpdateImage(swordGirl1Image);
        characterName.text = "ĳ���� :  Sword1";
        characterBestScore.text = "�ְ���� : " + DataManager.Instance.swordGirl1.bestScore.ToString();
        characterKillCount.text = "ųī��Ʈ : " + DataManager.Instance.swordGirl1.totalKillScore.ToString();
    }

    public void SelectSwordGirl2()
    {
        if (DataManager.Instance.playerData.SwordGirl2Get)
        { // �������2�� �����ϰ� �ִٸ�
            swordGirl1 = false;
            swordGirl2 = true;
            swordGirl3 = false;
            leon = false;
        }
        //UpdateImage(swordGirl2Image);
        characterName.text = "ĳ���� :  Sword2";
        characterBestScore.text = "�ְ���� : " + DataManager.Instance.swordGirl2.bestScore.ToString();
        characterKillCount.text = "ųī��Ʈ : " + DataManager.Instance.swordGirl2.totalKillScore.ToString();
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
        else
        {
            swordGirl3Purchase.gameObject.SetActive(true);
        }
        //UpdateImage(swordGirl2Image);
        characterName.text = "ĳ���� :  Sword3";
        characterBestScore.text = "�ְ���� : " + DataManager.Instance.swordGirl3.bestScore.ToString();
        characterKillCount.text = "ųī��Ʈ : " + DataManager.Instance.swordGirl3.totalKillScore.ToString();
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
        else
        {
            leonPurchase.gameObject.SetActive(true);
        }
        //UpdateImage(leonImage);
        characterName.text = "ĳ���� :  Leon";
        characterBestScore.text = "�ְ���� : " + DataManager.Instance.leon.bestScore.ToString();
        characterKillCount.text = "ųī��Ʈ : " + DataManager.Instance.leon.totalKillScore.ToString();
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

    public void PurchaseSwordGirl3()
    {
        if (DataManager.Instance.playerData.Gold >= 250 && DataManager.Instance.playerData.SwordGirl3Get == false)
        {
            DataManager.Instance.playerData.Gold -= 250;
            DataManager.Instance.playerData.SwordGirl3Get = true;
            purchaseSuccess.gameObject.SetActive(true);
            DataManager.Instance.SaveGameData();
            playerGold.text = DataManager.Instance.playerData.Gold.ToString();
            swordGirl3Image.sprite = swordGirl3Sprite;
            BackEndManager.Instance.DbSaveGameData();
        }
        else
        {
            swordGirl3PurchaseText.gameObject.SetActive(false);
            goldLackText.gameObject.SetActive(true);
        }
    }

    public void PurchaseLeon()
    {
        if (DataManager.Instance.playerData.Gold >= 500 && DataManager.Instance.playerData.LeonGet == false)
        {
            DataManager.Instance.playerData.Gold -= 500;
            DataManager.Instance.playerData.LeonGet = true;
            purchaseSuccess.gameObject.SetActive(true);
            DataManager.Instance.SaveGameData();
            playerGold.text = DataManager.Instance.playerData.Gold.ToString();
            leonImage.sprite = leonSprite;
            BackEndManager.Instance.DbSaveGameData();
        }
        else
        {
            leonPurchaseText.gameObject.SetActive(false);
            goldLackText1.gameObject.SetActive(true);
        }
    }

    public void SwordGirl3PurchaseUiExit()
    {
        swordGirl3PurchaseText.gameObject.SetActive(true);
        goldLackText.gameObject.SetActive(false);
        swordGirl3Purchase.SetActive(false);
    }

    public void LeonPurchaseUiExit()
    {
        leonPurchaseText.gameObject.SetActive(true);
        goldLackText1.gameObject.SetActive(false);
        leonPurchase.SetActive(false);
    }

    public void ExitUIButton()
    {
        purchaseSuccess.gameObject.SetActive(false);
        swordGirl3Purchase.gameObject.SetActive(false);
        leonPurchase.gameObject.SetActive(false);
    }
}
