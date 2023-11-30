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
        // �̹� �ν��Ͻ��� �����ϴ� ��� ������ ������Ʈ�� �ı�
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // �� ��ȯ �ÿ��� �ı����� �ʵ��� ����
        DontDestroyOnLoad(gameObject);

        // ���� ������Ʈ�� �ν��Ͻ��� ����
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
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // ������� ��ȣ�� ����
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

        // ���̵��� �� ��� �ð�
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

                // CanvasGroup�� ���ٸ� ������Ʈ �߰�
                if (CanvasGroup == null)
                {
                    CanvasGroup = imageObject.AddComponent<CanvasGroup>();
                }

                // �߰��� CanvasGroup�� ���� ���� ���⿡ ������ �� �ֽ��ϴ�.
                
                hasInitialized = true;
            }
            Debug.Log("�ڷ�ƾ ����");
            StartCoroutine(FadeOutCanvasGroup());
        }
    }
    IEnumerator FadeOutCanvasGroup()
    {
        float fadeDuration = 2.5f; // ���÷� 2�ʷ� �ø�
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
