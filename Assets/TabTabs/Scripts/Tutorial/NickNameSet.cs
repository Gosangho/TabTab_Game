using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

namespace TabTabs.NamChanwoo
{
  public class NickNameSet : MonoBehaviour
  {
      [SerializeField] private GameObject nameCanvas; //튜토리얼 캔버스
      [SerializeField] private GameObject nextCanvas; //튜토리얼 캔버스

      [SerializeField] private GameObject LeonObject; //튜토리얼 레온
      [SerializeField] private GameObject PlayerObject; // 튜토리얼 플레이어
      [SerializeField] private GameObject BackGroundCanvas; //튜토리얼 배경 캔버스
      [SerializeField] private GameObject InputField;
      [SerializeField] private GameObject messageBox;  // 메시지 박스
      [SerializeField] private GameObject messageErrBox;  // 에러 메시지 컴포넌트


      TextMeshProUGUI displayText; // 입력을 표시할 TextMeshPro 텍스트 컴포넌트
      TMP_InputField inputTexts; 
      DialogTest dialogTest;
      private string inputString = ""; // 사용자 입력을 저장할 문자열

      // Start is called before the first frame update
      void Start()
      {
        displayText = InputField.GetComponent<TextMeshProUGUI>();
        inputTexts = InputField.GetComponent<TMP_InputField>();

        if(DataManager.Instance.playerData.MakeNickName==true) { // 닉네임 생성 여부 확인
          decideButtonEvent();
        }
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
        if(displayText.text.Length > 1){
          string resultBool = BackEndManager.Instance.DblanguageCheckData(displayText.text);
          
          if("".Equals(resultBool)) {
            DataManager.Instance.playerData.MakeNickName = true;
            DataManager.Instance.playerData.PlayerName = displayText.text;
            BackEndManager.Instance.DbSaveGameData();
            BackEndManager.Instance.DbSaveNickname(displayText.text);
            messageBox.SetActive(true);
          } else if ("filterFalse".Equals(resultBool)) {
            messageErrBox.SetActive(true);
            messageErrBox.GetComponent<TextMeshProUGUI>().text = "비속어가 포함되 있습니다.";
          } else if ("existName".Equals(resultBool)) {
            messageErrBox.SetActive(true);  
            messageErrBox.GetComponent<TextMeshProUGUI>().text = "같은 이름이 존재합니다.";
          } else {
            messageErrBox.SetActive(true); 
          }
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
}
