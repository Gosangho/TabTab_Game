using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldText : MonoBehaviour
{
    TextMeshProUGUI HpText;
    // Start is called before the first frame update
    void Start()
    {
        HpText = GetComponent<TextMeshProUGUI>();
        Debug.Log("gold::"+DataManager.Instance.playerData.Gold);
        HpText.text = DataManager.Instance.playerData.Gold.ToString();
    }

}
