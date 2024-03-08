using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class NickNameSet : MonoBehaviour
{
    [SerializeField] private GameObject nameCanvas; //튜토리얼 캔버스
    [SerializeField] private GameObject nextCanvas; //튜토리얼 캔버스

    [SerializeField] private GameObject LeonObject; //튜토리얼 레온
    [SerializeField] private GameObject PlayerObject; // 튜토리얼 플레이어
    [SerializeField] private GameObject BackGroundCanvas; //튜토리얼 배경 캔버스
    [SerializeField] private GameObject InputField;
    [SerializeField] private GameObject messageBox;  // 버튼 컴포넌트

    TextMeshProUGUI displayText; // 입력을 표시할 TextMeshPro 텍스트 컴포넌트
    TMP_InputField inputTexts; 
    private string inputString = ""; // 사용자 입력을 저장할 문자열

    // Start is called before the first frame update
    void Start()
    {
      displayText = InputField.GetComponent<TextMeshProUGUI>();
      inputTexts = InputField.GetComponent<TMP_InputField>();
    }

  
    public void inputText(string text)
    {
      Debug.Log("inputText : " + text + "/"+ text.Length);
        // Backspace 키 처리
      if (text.Length != 0)
      {
        if(text.Length > 8) {
          displayText.text = text.Substring(0, text.Length - 1);
          inputTexts.text = text.Substring(0, text.Length - 1);
        }
      }
    }

    // 입력값 욕설 확인
    public void InputbuttonEvent()
    {
      bool resultBool = BackEndManager.Instance.DblanguageCheckData(displayText.text);

      if(resultBool) {
        Debug.Log("욕설입니다.");
      } else {
        DataManager.Instance.playerData.MakeNickName = true;
        DataManager.Instance.playerData.PlayerName = displayText.text;
        BackEndManager.Instance.DbSaveGameData();
        BackEndManager.Instance.DbSaveGameData(displayText.text);
        messageBox.SetActive(true);
      }
    }

    // 닉네임 생성
    public void decideButtonEvent()
    {
      nameCanvas.SetActive(false);
      nextCanvas.SetActive(true);
      LeonObject.SetActive(true); //튜토리얼 레온
      PlayerObject.SetActive(true); // 튜토리얼 플레이어
      BackGroundCanvas.SetActive(true); //튜토리얼 배경 캔버스
    }
}
