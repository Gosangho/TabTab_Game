using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlAttandSprite : MonoBehaviour
{
    public Image currentImage;
    [SerializeField] Sprite toDayAttand; // 오늘의 출석보상
    [SerializeField] Sprite lateAttand; // 아직 날짜가 안된 출석보상
    [SerializeField] Sprite notGetAttand; // 날짜가 지나서 얻지못한 출석보상
    public Button attandRewardCharacterButton;
    public Button attandRewardGoldButton;
    public Sprite attandRewardCharacterSprite;
    public Sprite attandRewardGoldSprite;

    void Start()
    {
        currentImage = GetComponent<Image>();

        int day = int.Parse(gameObject.name);

        if (day < System.DateTime.Now.Day && AttandManager.AttandInstance.attandDay[day - 1] == false)
            currentImage.sprite = lateAttand;
        else if (day < System.DateTime.Now.Day && AttandManager.AttandInstance.attandDay[day - 1] == true)
            currentImage.sprite = toDayAttand;
        else if (day == System.DateTime.Now.Day && AttandManager.AttandInstance.attandDay[day - 1] == true)
            currentImage.sprite = toDayAttand;
        else
            currentImage.sprite = notGetAttand;
    }

    public void UpdateSprite()
    {
        if (currentImage.sprite != toDayAttand)
        {
            currentImage.sprite = toDayAttand;
            DataManager.Instance.playerData.Gold += 10;
            AttandManager.AttandInstance.attandCount++;
            DataManager.Instance.SaveGameData();
            Debug.Log("오늘의 출석보상 수령");
            if (AttandManager.AttandInstance.attandCount >= 14)
            {
                attandRewardCharacterButton.image.sprite = attandRewardCharacterSprite;
                attandRewardGoldButton.image.sprite = attandRewardGoldSprite;
            }
        }
    }
}
