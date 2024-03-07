using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class NickNameSet : MonoBehaviour
{
    [SerializeField] private GameObject nextCanvas; //튜토리얼 캔버스

    [SerializeField] private GameObject LeonObject; //튜토리얼 레온
    [SerializeField] private GameObject PlayerObject; // 튜토리얼 플레이어
    [SerializeField] private GameObject BackGroundCanvas; //튜토리얼 배경 캔버스
    [SerializeField] private GameObject InputField;

    public TextMeshProUGUI displayText; // 입력을 표시할 TextMeshPro 텍스트 컴포넌트
    private string inputString = ""; // 사용자 입력을 저장할 문자열

    // Start is called before the first frame update
    void Start()
    {
        Transform outputTransform = InputField.transform.Find("Output");

        displayText = outputTransform.GetComponent<TextMeshProUGUI>();
    }

  
    public void inputText(string text)
    {
        Debug.Log("inputText : " + text);
        // Backspace 키 처리
       if (displayText.text.Length != 0)
        {
           if(displayText.text.Length > 8) {
                displayText.text = displayText.text.Substring(0, displayText.text.Length - 1);
           }
        }
    }
}
