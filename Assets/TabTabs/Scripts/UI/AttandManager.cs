using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class AttandManager : MonoBehaviour
{
    static protected AttandManager s_AttandInstance;
    static public AttandManager AttandInstance { get { return s_AttandInstance; } }

    
    [Header("출석일자")]
    public bool[] attandDay = new bool[28];

    [Header("14일 보상 버튼")]
    public Button day14_attandanceCharacterReward;
    public Button day14_DattandanceGoldReward;

    [Header("14일 보상 획득가능 이미지")]
    public Sprite day14_onAttandanceCharacterRewardSprite;
    public Sprite day14_onAttandanceGoldRewardSprite;

    [Header("14일 보상을 획득했을때의 이미지")]
    public Sprite day14_getAttandanceCharacterRewardSprite;
    public Sprite day14_getAttandanceGoldRewardSprite;

    [Header("28일 보상 버튼")]
    public Button day28_attandanceCharacterReward;
    public Button day28_DattandanceGoldReward;
    
    [Header("28일 보상 획득가능 이미지")]
    public Sprite day28_onAttandanceCharacterRewardSprite;
    public Sprite day28_onAttandanceGoldRewardSprite;

    [Header("28일 보상을 획득했을때의 이미지")]
    public Sprite day28_getAttandanceCharacterRewardSprite;
    public Sprite day28_getAttandanceGoldRewardSprite;

    public int attandCount;

    public bool day14_getattandanceCharacterReward;
    public bool day14_getattandanceGoldReward;

    public bool day28_getattandanceCharacterReward;
    public bool day28_getattandanceGoldReward;

    private void Awake()
    {
        s_AttandInstance = this;
        attandDay = DataManager.Instance.playerData.PlayerAttandence;

        if (DataManager.Instance.playerData.SwordGirl2Get == true)
        {
            day14_getattandanceCharacterReward = true;
            day14_getattandanceGoldReward = true;
        }
        
        if (DataManager.Instance.playerData.SwordGirl3Get == true)
        {
            day28_getattandanceCharacterReward = true;
            day28_getattandanceGoldReward = true;
        }
    }

    private void Start()
    {
        for (int i = 0; i < attandDay.Length; i++)
        {
            if (attandDay[i])
            {
                attandCount++;
            }
        }
        Debug.Log("True Count: " + attandCount);
        
        if (attandCount >= 14 && day14_getattandanceCharacterReward == false && day14_getattandanceGoldReward == false)
        {
            day14_attandanceCharacterReward.image.sprite = day14_onAttandanceCharacterRewardSprite;
            day14_DattandanceGoldReward.image.sprite = day14_onAttandanceGoldRewardSprite;
        }
        else if (attandCount >= 14 && day14_getattandanceCharacterReward == true && day14_getattandanceGoldReward == true)
        {
            day14_attandanceCharacterReward.image.sprite = day14_getAttandanceCharacterRewardSprite;
            day14_DattandanceGoldReward.image.sprite = day14_getAttandanceGoldRewardSprite;
        }

        if (attandCount >= 28 && day28_getattandanceCharacterReward == false && day28_getattandanceGoldReward == false)
        {
            day28_attandanceCharacterReward.image.sprite = day28_onAttandanceCharacterRewardSprite;
            day28_DattandanceGoldReward.image.sprite = day28_onAttandanceGoldRewardSprite;
        }
        else if (attandCount >= 28 && day28_getattandanceCharacterReward == true && day28_getattandanceGoldReward == true)
        {
            day28_attandanceCharacterReward.image.sprite = day28_getAttandanceCharacterRewardSprite;
            day28_DattandanceGoldReward.image.sprite = day28_getAttandanceGoldRewardSprite;
        }
    }

    public void GetAttandCharacter()
    {
        if (attandCount >= 14 && day14_getattandanceCharacterReward == false)
        {
            day14_attandanceCharacterReward.image.sprite = day14_getAttandanceCharacterRewardSprite;
            DataManager.Instance.playerData.SwordGirl2Get = true;
            DataManager.Instance.SaveGameData();
            Debug.Log("14일 출석 캐릭터 보상" + DataManager.Instance.playerData.SwordGirl2Get);
            day14_getattandanceCharacterReward = true;
        }
        else if (attandCount >= 28 && day28_getattandanceCharacterReward == false)
        {
            day28_attandanceCharacterReward.image.sprite = day28_getAttandanceCharacterRewardSprite;
            DataManager.Instance.playerData.SwordGirl3Get = true;
            DataManager.Instance.SaveGameData();
            Debug.Log("28일 출석 캐릭터 보상" + DataManager.Instance.playerData.SwordGirl3Get);
            day28_getattandanceCharacterReward = true;
        }
    }

    public void GetAttandGold()
    {
        if (attandCount >= 14 && day14_getattandanceGoldReward == false)
        {
            day14_DattandanceGoldReward.image.sprite = day14_getAttandanceGoldRewardSprite;
            DataManager.Instance.playerData.Gold += 100;
            DataManager.Instance.SaveGameData();
            Debug.Log("14일 출석 골드 보상" + DataManager.Instance.playerData.Gold);
            day14_getattandanceGoldReward = true;
        }
        else if (attandCount >= 28 && day28_getattandanceGoldReward == false)
        {
            day28_DattandanceGoldReward.image.sprite = day28_getAttandanceGoldRewardSprite;
            DataManager.Instance.playerData.Gold += 100;
            DataManager.Instance.SaveGameData();
            Debug.Log("28일 출석 골드 보상" + DataManager.Instance.playerData.Gold);
            day28_getattandanceGoldReward = true;
        }
    }
}
