using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private static SceneChange instance;

    void Awake()
    {
        // 이미 인스턴스가 존재하는 경우 현재의 오브젝트를 파괴
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // 씬 전환 시에도 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);

        // 현재 오브젝트를 인스턴스로 설정
        instance = this;
    }
    public Button LobbyButton;
    private int currentSceneIndex;
    public CanvasGroup CanvasGroup;
    public int SceneNumber;
    public bool cy;
    private bool hasInitialized = false;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // 현재씬의 번호를 저장
        cy = false;
        
    }

    public void StartFade()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(_SceneChange());
    }

    IEnumerator _SceneChange()
    {
        float elapsedTime = 0f;

        while (CanvasGroup.alpha < 1f)
        {
            elapsedTime += Time.deltaTime;
            CanvasGroup.alpha = Mathf.Clamp01(elapsedTime / 1f);
            yield return null;
        }

        // 페이드인 후 대기 시간
        yield return new WaitForSeconds(0.5f);
        cy = true;
        SceneManager.LoadScene(4);
    }
    private void Update()
    {
        SceneNumber = SceneManager.GetActiveScene().buildIndex;
        if (!hasInitialized && SceneNumber != currentSceneIndex && cy && CanvasGroup == null)
        {
            GameObject imageObject = GameObject.Find("Image");
            if (imageObject != null)
            {
                CanvasGroup = imageObject.GetComponent<CanvasGroup>();

                // CanvasGroup이 없다면 컴포넌트 추가
                if (CanvasGroup == null)
                {
                    CanvasGroup = imageObject.AddComponent<CanvasGroup>();
                }

                // 추가된 CanvasGroup의 설정 등을 여기에 수행할 수 있습니다.
                
                hasInitialized = true;
            }
            Debug.Log("코루틴 시작");
            StartCoroutine(FadeOutCanvasGroup());
        }
    }
    IEnumerator FadeOutCanvasGroup()
    {
        float fadeDuration = 2.5f; // 예시로 2초로 늘림
        float startAlpha = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            CanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
            yield return null;
        }
    }
}
