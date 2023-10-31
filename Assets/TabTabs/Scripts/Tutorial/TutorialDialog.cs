using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialDialog : MonoBehaviour
{
    public TextMeshProUGUI dialogText;  // UI Text 요소
    public TutorialButtonEffect TutorialButtonEffect;
    public Button AttackButton;
    private string[] dialogLines;  // 대화 문장 배열
    [SerializeField]public int currentLine = 0;   // 현재 대화 인덱스

    void Start()
    {
        TutorialButtonEffect = FindObjectOfType<TutorialButtonEffect>();
        // 대화 문장 초기화
        dialogLines = new string[]
        {
            "안녕하세요! 용사님의 튜토리얼을 맡게된 000 입니다.", // 튜토리얼 캐릭터가 있어야하나 ?
            "몬스터를 처치하기 위해서는",
            "오른쪽 몬스터부터 공격버튼을 눌러서 처리해야 합니다.",
            // 공격버튼이 깜빡이게 된다.
            "몬스터의 공격포인트를 확인하고 신중하게 공격하세요 !",
            "이제 공격버튼을 터치 해보세요 !",
            // 공격버튼을 누르게된다면 깜빡이는 효과 off
            "몬스터의 공격포인트 이상의 공격을 하게 된다면",
            "게임이 끝나게 됩니다.",
            // 사용자의 오른쪽몬스터 공격 유도
            "오른쪽 몬스터를 처리했다면, 대쉬버튼을 눌러서",
            // 대쉬버튼이 깜빡이게 된다.
            "왼쪽 몬스터에 이동공격 후 왼쪽 몬스터를",
            "처치하세요!",
            // 왼쪽 몬스터 처치 후 
            "왼쪽 몬스터를 처치했다면 다시 오른쪽 몬스터를",
            "처치하는 방식으로 진행되게 된답니다",
            // 1. Timebar쪽이 깜빡이게 된다. 2. 1초후 Timebar의 시간이 줄어들게 된다.
            "앗! 상단에 시간이 줄고있어요",
            // 튜토리얼에서는 Timebar가 50%로 시작해서 최대 10%까지만 줄어들게 설정
            "시간이 끝나게되면 마찬가지로 게임이 끝나게 됩니다.",
            "몬스터의 공격포인트를 공격하면 시간이 늘어나게 된답니다.",
            "최대한 많은 몬스터를 처치하고 점수를 획득해서",
            "최고기록을 세워보세요 !",
            "이상으로 튜토리얼을 마치겠습니다!",
            "용사님의 건투를 빕니다!"
            // 1. Skip버튼이 Go버튼으로 변경 2. Go버튼이 깜빡이게 된다. 3. Go버튼을 누르면 로비씬으로 이동
        };

        // 초기 대화 표시
        ShowDialog();
    }

    void Update()
    {
        // '다음' 버튼을 누르면 다음 대화로 진행
        if (Input.GetMouseButtonDown(0)) // 모바일환경으로 변환시 Input.touchCount > 0 (테스트 후 변경예정)
        {
            NextLine();
        }
    }

    void NextLine()
    {
        if (currentLine < dialogLines.Length - 1)
        {
            currentLine++;
            ShowDialog();
        }
        else
        {
            // 대화가 끝났을 때 처리 (예: 대화창 닫기)
            Debug.Log("대화 종료");
        }
    }

    void ShowDialog()
    {
        string currentDialog = dialogLines[currentLine];
        // 현재 대화 문장을 텍스트 UI에 표시
        dialogText.text = dialogLines[currentLine];
    }

}
