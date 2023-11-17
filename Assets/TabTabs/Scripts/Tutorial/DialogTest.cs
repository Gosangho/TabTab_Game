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
        public Image DashImage;
        public Image TimebarImage;
        public Image TimebarBackGroundImage;
        public GameObject Timebar;
        public TextMeshProUGUI SkipButtonText;
        public GameObject BattleObj;
        private float halfAnimationTime = 0.4f;
        public TutorialBattleSystem TutorialBattleSystem;
        bool ObjActive = false;
        bool Istrue = false;

        private void Start()
        {
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
            StartCoroutine(ChangeAlphaOverTime(AttackImage, 1f, 0.1f));
            yield return new WaitForSeconds(halfAnimationTime);
            StartCoroutine(ChangeAlphaOverTime(AttackImage, 0.1f, 1f));

            yield return new WaitForSeconds(2);
            // 두 번째 대사 분기 시작
            yield return new WaitUntil(() => dialogSystem02.UpdateDialog());

            yield return new WaitForSeconds(0.2f);

            //StartCoroutine(ChangeAlphaOverTime2());
            StartCoroutine(ChangeAlphaOverTime(DashImage, 1f, 0.1f));
            yield return new WaitForSeconds(halfAnimationTime);
            StartCoroutine(ChangeAlphaOverTime(DashImage, 0.1f, 1f));

            yield return new WaitForSeconds(2);

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

            BattleObj.gameObject.SetActive(true);

            //TutorialBattleSystem = FindObjectOfType<TutorialBattleSystem>();

            //yield return new WaitUntil(() => TutorialBattleSystem.MonsterDie);
            // 2. 몬스터 쓰러트림
            yield return new WaitUntil(()=>Istrue); // Istrue변수가 true일때까지 대기
            // 3. SkipButton의 Text를 Go로 변경
            SkipButtonText.text = "Go";
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


