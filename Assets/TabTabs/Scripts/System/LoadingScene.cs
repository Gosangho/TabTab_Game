using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using GooglePlayGames;

namespace TabTabs.NamChanwoo
{
    public class LoadingScene : MonoBehaviour
    {
        public GameObject loadingSliderBarObject;
        public Slider loadingSliderBar;
        public TextMeshProUGUI loadingNumberText;
        public AsyncOperation asyncOperation;

        public GameObject loadingImage;

        public Camera cam;

        string sceneNameToLoad = "Opening"; 
        string sceneNameToLoby = "lobby"; 

        void Awake()
        {
            //구글 로그인
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
        }


        void Start()
        {
            ContinueButton.continueButtonClick = false;
         //   DataManager.Instance.SaveGameData();
            Debug.Log("로딩 시작");
           
            if(FadeScene.isLogin) {
                loadingSliderBarObject.SetActive(false);
                loadingNumberText.text = "로그인 중입니다.";
                StartCoroutine(LoginLoading());     
            } else {
                StartCoroutine(Loading());
            }
        }


        IEnumerator LoginLoading()
        {
            yield return new WaitForSeconds(1f);
            
            if(googleLogin()) {
                BackEndManager.Instance.broInit();
                SceneManager.LoadScene(sceneNameToLoby);
            }
        }

       


        IEnumerator Loading()
        {
            if (FadeScene.isLobby)
            {
                asyncOperation = SceneManager.LoadSceneAsync("TabTabs/Scenes/lobby");
                asyncOperation.allowSceneActivation = false;
                FadeScene.isLobby = false;
            }
            else if (FadeScene.isBattle)
            {
                asyncOperation = SceneManager.LoadSceneAsync("TabTabs/Scenes/Test3Battle 1");
                asyncOperation.allowSceneActivation = false;
                FadeScene.isBattle = false;
            }
            else if (FadeScene.isBattle2)
            {
                asyncOperation = SceneManager.LoadSceneAsync("TabTabs/Scenes/Test4Battle");
                asyncOperation.allowSceneActivation = false;
                FadeScene.isBattle = false;
            }
            else if (FadeScene.isTutorial)
            {
                asyncOperation = SceneManager.LoadSceneAsync("TabTabs/Scenes/Tutorial");
                asyncOperation.allowSceneActivation = false;
                FadeScene.isTutorial = false;
            }


            float duration = 5f; 
            float targetValue = 1f;
            float startTime = Time.time;

            while (loadingSliderBar.value < targetValue)
            {
                float progress = (Time.time - startTime) / duration;
                loadingSliderBar.value = progress;

                float scaleValue = loadingSliderBar.value * 100f;
                int intValue = Mathf.RoundToInt(scaleValue);
                loadingNumberText.text = intValue.ToString(); 

                yield return null;
            }

            loadingSliderBar.value = targetValue;
            int finalValue = Mathf.RoundToInt(targetValue * 100f);
            loadingNumberText.text = finalValue.ToString(); 

            
            cam.rect = new Rect(0, 0, 1, 1);
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.black;

            loadingImage.SetActive(true);

            yield return new WaitForSeconds(0.1f);
           
            asyncOperation.allowSceneActivation = true;

        }

        bool googleLogin() {
            bool isLogin = false;
            string userId;

            #if UNITY_EDITOR
                Debug.Log("구글 로그인 성공");
                isLogin = true;
            #else
                if(PlayGamesPlatform.Instance.localUser.authenticated){
                    Debug.Log("이미 로그인 되어있음");
                    isLogin = true;
                } else {
                    Social.localUser.Authenticate((bool success) => {
                        if(success){
                            Debug.Log("구글 로그인 성공");
                            userId = Social.localUser.id;
                            isLogin = true;
                        } else {
                            Debug.Log("구글 로그인 실패");
                            isLogin = false;
                        }
                    });
                }
            #endif

            return isLogin;
        }
    }
}

