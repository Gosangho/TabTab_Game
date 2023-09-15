using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeart : MonoBehaviour
{
    public int PlayerHeartGauge = 1; // 플레이어 하트 갯수
    PlayerBase PlayerBaseInstance;
    void Start()
    {
        PlayerBaseInstance = FindObjectOfType<PlayerBase>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHeartGauge <= 0)
        {
            PlayerBaseInstance.PlayerAnim.SetTrigger("Die");
            Destroy(gameObject);
        }
    }
}
