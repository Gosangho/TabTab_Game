using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttandButton : MonoBehaviour
{
    private ControlAttandSprite[] rewardSprite;
    [SerializeField]
    private GameObject rewards;

    void Start()
    {
        rewardSprite = rewards.GetComponentsInChildren<ControlAttandSprite>();
    }

    public void ChangingSprite()
    {
        rewardSprite[System.DateTime.Now.Day - 1].UpdateSprite();
        AttandManager.AttandInstance.attandDay[System.DateTime.Now.Day - 1] = true;
    }
}
