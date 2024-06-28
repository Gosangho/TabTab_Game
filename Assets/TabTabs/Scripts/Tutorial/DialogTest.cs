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
        [SerializeField]
        private GameObject skipButton;
        [SerializeField] private GameObject tutorialCanvas;  // 버튼 컴포넌트   

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
        bool tutorialStart = false;

        
        private void Start()
        {
            SkipButtonInstance = skipButton.GetComponent<SkipButton>();
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
            if(audioManager.Instance.SfxAudio_Enemy_hitAudio != null) {
                audioManager.Instance.SfxAudio_Enemy_hitAudio.loop = true;
                audioManager.Instance.SfxAudioPlay_Enemy("Tutorial_Warning");
                yield return new WaitForSeconds(3.0f);
                audioManager.Instance.SfxAudio_Enemy_hitAudio.loop = false;
            }
        }
        public IEnumerator StartDialog()
        {
            // ù ��° ���б� ����
            yield return new WaitUntil(() => dialogSystem01.UpdateDialog());
            yield return new WaitForSeconds(0.2f);
            // ��� �б� ���̿� ���ϴ� �ൿ�� �߰�
            //StartCoroutine(ChangeAlphaOverTime());
            AttackTextImage.gameObject.SetActive(true);
            StartCoroutine(ChangeAlphaOverTime(AttackImage, 1f, 0.1f));
            StartCoroutine(ChangeAlphaOverTime(AttackTextImage, 1f, 0.1f));
            yield return new WaitForSeconds(halfAnimationTime);
            StartCoroutine(ChangeAlphaOverTime(AttackImage, 0.1f, 1f));
            StartCoroutine(ChangeAlphaOverTime(AttackTextImage, 0.1f, 1f));
            yield return new WaitForSeconds(2);
            AttackTextImage.gameObject.SetActive(false);
            // �� ��° ��� �б� ����
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

            Timebar.gameObject.SetActive(true); // Ÿ�ӹ� Ȱ��ȭ

            StartCoroutine(ChangeAlphaOverTime(TimebarImage, 1f, 0.1f));
            StartCoroutine(ChangeAlphaOverTime(TimebarBackGroundImage, 1f, 0.1f));
            yield return new WaitForSeconds(halfAnimationTime);
            StartCoroutine(ChangeAlphaOverTime(TimebarImage, 0.1f, 1f));
            StartCoroutine(ChangeAlphaOverTime(TimebarBackGroundImage, 0.1f, 1f));

            yield return new WaitForSeconds(2);

            yield return new WaitUntil(() => dialogSystem04.UpdateDialog());

            // 1. ���� ��
            ObjActive = true;
            MonsterSpawn = true;
            BattleObj.gameObject.SetActive(true);
            AttackTextImage.gameObject.SetActive(true);
            DashTextImage.gameObject.SetActive(true);
            GuideTextBackgroundImage.gameObject.SetActive(true);
            StartCoroutine(FadeObjectsRoutine());
            yield return new WaitUntil(() => FirstAttack); // �����Ҷ����� �ݺ�

            //TutorialBattleSystem = FindObjectOfType<TutorialBattleSystem>();

            //yield return new WaitUntil(() => TutorialBattleSystem.MonsterDie);
            // 2. ���� ����Ʈ��
            yield return new WaitUntil(()=>Istrue); // Istrue������ true�϶����� ���

            AttackImage.gameObject.SetActive(false);
            AttackTextImage.gameObject.SetActive(false);
            DashImage.gameObject.SetActive(false);
            DashTextImage.gameObject.SetActive(false);
            // 3. SkipButton�� Image�� GoImage�� ����

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
                // Ư�� ������ ������ �� ���̵� �� �Ǵ� ���̵� �ƿ� ����
                if (FirstAttack == false)
                {
                    float alpha = Mathf.PingPong(Time.time * fadeSpeed, 1f); // 0~1 ������ ���İ��� �ݺ�

                    // Image ������Ʈ�� ���İ� ����
                    Color imageColor = GuideTextBackgroundImage.color;
                    imageColor.a = alpha;
                    GuideTextBackgroundImage.color = imageColor;

                    // TextMeshProUGUI ������Ʈ�� ���İ� ����
                    Color textColor = GuideText.color;
                    textColor.a = alpha;
                    GuideText.color = textColor;
                }
                else
                {
                    GuideTextBackgroundImage.gameObject.SetActive(false);
                }

                yield return null; // �� �������� ��ٸ��ϴ�.
            }
        }
    }
}

        //private IEnumerator ChangeAlphaOverTime()
        //{
        //    for (int iteration = 0; iteration < 3; iteration++)
        //    {
        //        // ù ��° �ݺ�: ���İ��� 1���� 0.1�� ����
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

        //        // ���� ���İ��� 0.1f�� ���� (���� �̹� 0.1f�� ���������� ���������� ����)
        //        Color finalColor = AttackImage.color;
        //        finalColor.a = 0.1f;
        //        AttackImage.color = finalColor;

        //        yield return new WaitForSeconds(halfAnimationTime);

        //        // �� ��° �ݺ�: ���İ��� 0.1���� 1�� ����
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

        //        // ���� ���İ��� 1f�� ����
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
        //        // ù ��° �ݺ�: ���İ��� 1���� 0.1�� ����
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

        //        // ���� ���İ��� 0.1f�� ���� (���� �̹� 0.1f�� ���������� ���������� ����)
        //        Color finalColor = DashImage.color;
        //        finalColor.a = 0.1f;
        //        DashImage.color = finalColor;

        //        yield return new WaitForSeconds(halfAnimationTime);

        //        // �� ��° �ݺ�: ���İ��� 0.1���� 1�� ����
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

        //        // ���� ���İ��� 1f�� ����
        //        finalColor = DashImage.color;
        //        finalColor.a = 1f;
        //        DashImage.color = finalColor;

        //        yield return new WaitForSeconds(halfAnimationTime);
        //    }
        //}}


