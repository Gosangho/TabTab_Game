using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButtonEffect : MonoBehaviour
{
    public Button button;  // UI 버튼
    public Sprite NormalImage;
    public Sprite AfterImage;
    private Image ButtonImage;
    private bool isNormalImage = true;
    public bool Repeat = true;
    public TutorialDialog TutorialDialog;
    private void Start()
    {
        TutorialDialog = FindObjectOfType<TutorialDialog>();
        ButtonImage = button.GetComponent<Image>();
        ButtonImage.sprite = NormalImage;
        StartCoroutine(SwitchImagesRepeatedly());
    }

    public IEnumerator SwitchImagesRepeatedly()
    {
        if (Repeat && TutorialDialog.currentLine == 2)
        {
            while (true)
            {
                // 이미지 전환
                if (isNormalImage)
                {
                    ButtonImage.sprite = AfterImage;
                }
                else
                {
                    ButtonImage.sprite = NormalImage;
                }

                // 상태 반전
                isNormalImage = !isNormalImage;

                // 0.5초 대기
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
