using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace TabTabs.NamChanwoo
{
    public class DialogTest : MonoBehaviour
    {
        [SerializeField]
        private DialogSystem dialogSystem01;
        [SerializeField]
        private DialogSystem dialogSystem02;
        [SerializeField]
        private DialogSystem dialogSystem03;
        [SerializeField]
        private DialogSystem dialogSystem04;
        [SerializeField]
        private DialogSystem dialogSystem05;
        public Image AttackImage;
        public Image AttackTextImage;
        public Image DashImage;
        public Image DashTextImage;
        public Image TimebarImage;
        public Image TimebarBackGroundImage;
        public GameObject Timebar;
        public Image GuideTextBackgroundImage;
        public TextMeshProUGUI GuideText;
        //public TextMeshProUGUI SkipButtonText;
        SkipButton SkipButtonInstance;
        public GameObject BattleObj;
        private float halfAnimationTime = 0.4f;
        public TutorialBattleSystem TutorialBattleSystem;
        bool ObjActive = false;
        bool Istrue = false;
        public bool FirstAttack = false;    
        float fadeSpeed = 1.5f;
        public bool MonsterSpawn = false;
        private void Start()
        {
            SkipButtonInstance = FindObjectOfType<SkipButton>();
            StartCoroutine(StartWarningAudio());
            StartCoroutine(StartDialog());
        }

        private void Update()
        {
            if (ObjActive)
            {
                TutorialBattleSystem = FindObjectOfType<TutorialBattleSystem>();
                if (TutorialBattleSystem.MonsterDie == true)
                {
                    Istrue = true;
                }
            }

            if (MonsterSpawn)
            {
                if (TutorialBattleSystem.selectEnemy != null && TutorialBattleSystem.selectEnemy.gameObject != null)
                {
                    float alpha1 = Mathf.PingPong(Time.time * fadeSpeed, 1f);

                    Color Attack = AttackImage.color;
                    Attack.a = alpha1;
                    AttackImage.color = Attack;

                    Color AttackText = AttackTextImage.color;
                    AttackText.a = alpha1;
                    AttackTextImage.color = AttackText;

                }
                else
                {
                    {
                        float alpha = Mathf.PingPong(Time.time * fadeSpeed, 1f);

                        Color Dash = DashImage.color;
                        Dash.a = alpha;
                        DashImage.color = Dash;

                        Color DashText = DashTextImage.color;
                        DashText.a = alpha;
                        DashTextImage.color = DashText;
                    }
                }
            }
        }

        public IEnumerator StartWarningAudio()
        {
            audioManager.Instance.SfxAudio_Enemy_hitAudio.loop = true;
            audioManager.Instance.SfxAudioPlay_Enemy("Tutorial_Warning");
            yield return new WaitForSeconds(3.0f);
            audioManager.Instance.SfxAudio_Enemy_hitAudio.loop = false;
        }
        public IEnumerator StartDialog()
        {
            // 첫 번째 대사분기 시작
            
            yield return new WaitUntil(() => dialogSystem01.UpdateDialog());
            yield return new WaitForSeconds(0.2f);
            // 대사 분기 사이에 원하는 행동을 추가
            //StartCoroutine(ChangeAlphaOverTime());
            AttackTextImage.gameObject.SetActive(true);
            StartCoroutine(ChangeAlphaOverTime(AttackImage, 1f, 0.1f));
            StartCoroutine(ChangeAlphaOverTime(AttackTextImage, 1f, 0.1f));
            yield return new WaitForSeconds(halfAnimationTime);
            StartCoroutine(ChangeAlphaOverTime(AttackImage, 0.1f, 1f));
            StartCoroutine(ChangeAlphaOverTime(AttackTextImage, 0.1f, 1f));
            yield return new WaitForSeconds(2);
            AttackTextImage.gameObject.SetActive(false);
            // 두 번째 대사 분기 시작
            yield return new WaitUntil(() => dialogSystem02.UpdateDialog());

            yield return new WaitForSeconds(0.2f);
            DashTextImage.gameObject.SetActive(true);
            //StartCoroutine(ChangeAlphaOverTime2());
            StartCoroutine(ChangeAlphaOverTime(DashImage, 1f, 0.1f));
            StartCoroutine(ChangeAlphaOverTime(DashTextImage, 1f, 0.1f));
            yield return new WaitForSeconds(halfAnimationTime);
            StartCoroutine(ChangeAlphaOverTime(DashImage, 0.1f, 1f));
            StartCoroutine(ChangeAlphaOverTime(DashTextImage, 0.1f, 1f));

            yield return new WaitForSeconds(2);
            DashTextImage.gameObject.SetActive(false);
            yield return new WaitUntil(() => dialogSystem03.UpdateDialog());

            Timebar.gameObject.SetActive(true); // 타임바 활성화

            StartCoroutine(ChangeAlphaOverTime(TimebarImage, 1f, 0.1f));
            StartCoroutine(ChangeAlphaOverTime(TimebarBackGroundImage, 1f, 0.1f));
            yield return new WaitForSeconds(halfAnimationTime);
            StartCoroutine(ChangeAlphaOverTime(TimebarImage, 0.1f, 1f));
            StartCoroutine(ChangeAlphaOverTime(TimebarBackGroundImage, 0.1f, 1f));

            yield return new WaitForSeconds(2);

            yield return new WaitUntil(() => dialogSystem04.UpdateDialog());

            // 1. 몬스터 젠
            ObjActive = true;
            MonsterSpawn = true;
            BattleObj.gameObject.SetActive(true);
            AttackTextImage.gameObject.SetActive(true);
            DashTextImage.gameObject.SetActive(true);
            GuideTextBackgroundImage.gameObject.SetActive(true);
            StartCoroutine(FadeObjectsRoutine());
            yield return new WaitUntil(() => FirstAttack); // 공격할때까지 반복

            //TutorialBattleSystem = FindObjectOfType<TutorialBattleSystem>();

            //yield return new WaitUntil(() => TutorialBattleSystem.MonsterDie);
            // 2. 몬스터 쓰러트림
            yield return new WaitUntil(()=>Istrue); // Istrue변수가 true일때까지 대기

            AttackImage.gameObject.SetActive(false);
            AttackTextImage.gameObject.SetActive(false);
            DashImage.gameObject.SetActive(false);
            DashTextImage.gameObject.SetActive(false);
            // 3. SkipButton의 Image를 GoImage로 변경

            SkipButtonInstance.Image.sprite = SkipButtonInstance.GoImage;

            yield return new WaitUntil(() => dialogSystem05.UpdateDialog());
        }

        private IEnumerator ChangeAlphaOverTime(Image targetImage, float startAlpha, float endAlpha)
        {
            for (int iteration = 0; iteration < 3; iteration++)
            {
                float currentTime = 0f;
                while (currentTime < halfAnimationTime)
                {
                    currentTime += Time.deltaTime;

                    float alpha = Mathf.Lerp(startAlpha, endAlpha, currentTime / halfAnimationTime);

                    Color color = targetImage.color;
                    color.a = alpha;
                    targetImage.color = color;

                    yield return null;
                }

                // Set the final alpha value
                Color finalColor = targetImage.color;
                finalColor.a = endAlpha;
                targetImage.color = finalColor;

                yield return new WaitForSeconds(halfAnimationTime);
            }
        }
        IEnumerator FadeObjectsRoutine()
        {
            while (true)
            {
                // 특정 조건을 만족할 때 페이드 인 또는 페이드 아웃 수행
                if (FirstAttack == false)
                {
                    float alpha = Mathf.PingPong(Time.time * fadeSpeed, 1f); // 0~1 사이의 알파값을 반복

                    // Image 오브젝트의 알파값 조절
                    Color imageColor = GuideTextBackgroundImage.color;
                    imageColor.a = alpha;
                    GuideTextBackgroundImage.color = imageColor;

                    // TextMeshProUGUI 오브젝트의 알파값 조절
                    Color textColor = GuideText.color;
                    textColor.a = alpha;
                    GuideText.color = textColor;
                }
                else
                {
                    GuideTextBackgroundImage.gameObject.SetActive(false);
                }

                yield return null; // 한 프레임을 기다립니다.
            }
        }
    }
}

        //private IEnumerator ChangeAlphaOverTime()
        //{
        //    for (int iteration = 0; iteration < 3; iteration++)
        //    {
        //        // 첫 번째 반복: 알파값을 1에서 0.1로 변경
        //        float currentTime = 0f;
        //        while (currentTime < halfAnimationTime)
        //        {
        //            currentTime += Time.deltaTime;

        //            float alpha = Mathf.Lerp(1f, 0.1f, currentTime / halfAnimationTime);

        //            Color color = AttackImage.color;
        //            color.a = alpha;
        //            AttackImage.color = color;

        //            yield return null;
        //        }

        //        // 최종 알파값을 0.1f로 설정 (보통 이미 0.1f로 도달하지만 명시적으로 설정)
        //        Color finalColor = AttackImage.color;
        //        finalColor.a = 0.1f;
        //        AttackImage.color = finalColor;

        //        yield return new WaitForSeconds(halfAnimationTime);

        //        // 두 번째 반복: 알파값을 0.1에서 1로 변경
        //        currentTime = 0f;
        //        while (currentTime < halfAnimationTime)
        //        {
        //            currentTime += Time.deltaTime;

        //            float alpha = Mathf.Lerp(0.1f, 1f, currentTime / halfAnimationTime);

        //            Color color = AttackImage.color;
        //            color.a = alpha;
        //            AttackImage.color = color;

        //            yield return null;
        //        }

        //        // 최종 알파값을 1f로 설정
        //        finalColor = AttackImage.color;
        //        finalColor.a = 1f;
        //        AttackImage.color = finalColor;

        //        yield return new WaitForSeconds(halfAnimationTime);
        //    }
        //}

        //private IEnumerator ChangeAlphaOverTime2()
        //{
        //    for (int iteration = 0; iteration < 3; iteration++)
        //    {
        //        // 첫 번째 반복: 알파값을 1에서 0.1로 변경
        //        float currentTime = 0f;
        //        while (currentTime < halfAnimationTime)
        //        {
        //            currentTime += Time.deltaTime;

        //            float alpha = Mathf.Lerp(1f, 0.1f, currentTime / halfAnimationTime);

        //            Color color = DashImage.color;
        //            color.a = alpha;
        //            DashImage.color = color;

        //            yield return null;
        //        }

        //        // 최종 알파값을 0.1f로 설정 (보통 이미 0.1f로 도달하지만 명시적으로 설정)
        //        Color finalColor = DashImage.color;
        //        finalColor.a = 0.1f;
        //        DashImage.color = finalColor;

        //        yield return new WaitForSeconds(halfAnimationTime);

        //        // 두 번째 반복: 알파값을 0.1에서 1로 변경
        //        currentTime = 0f;
        //        while (currentTime < halfAnimationTime)
        //        {
        //            currentTime += Time.deltaTime;

        //            float alpha = Mathf.Lerp(0.1f, 1f, currentTime / halfAnimationTime);

        //            Color color = DashImage.color;
        //            color.a = alpha;
        //            DashImage.color = color;

        //            yield return null;
        //        }

        //        // 최종 알파값을 1f로 설정
        //        finalColor = DashImage.color;
        //        finalColor.a = 1f;
        //        DashImage.color = finalColor;

        //        yield return new WaitForSeconds(halfAnimationTime);
        //    }
        //}}


