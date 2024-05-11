using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeart : MonoBehaviour
{
    public int PlayerHeartGauge = 3; // �÷��̾� ��Ʈ ����
    PlayerBase PlayerBaseInstance;
    void Start()
    {
        GameObject character2Object = GameObject.FindGameObjectWithTag("Player");
        if (character2Object != null)
        {
            PlayerBaseInstance = character2Object.GetComponent<PlayerBase>();
        }
    }

    
    void Update()
    {
        if (PlayerHeartGauge == 0)
        {
            PlayerBase.PlayerAnim.SetTrigger("Die");
            PlayerHeartGauge = -1;
            //Destroy(gameObject);
        }
    }
}
