using System.Collections;
using TabTabs.NamChanwoo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UISystem : GameSystem
{
    public Slider AttackSliderUI = null;
    
    public int comboCount = 0;
    
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI playerLifeText;

    private int playerLife = 75;

    private Vector3 originalPosition;
    private Vector3 originalLifeTextPosition;
    private Color originalColor;
    
    void Start()
    {
        
        GameManager.NotificationSystem.NodeHitSuccess.AddListener(HandleNodeHitSuccess);
        GameManager.NotificationSystem.NodeHitFail.AddListener(HandleNodeHitFail);

        originalPosition = comboText.transform.position;
        originalColor = comboText.color;
        
        playerLifeText.text = "x : " + playerLife.ToString();
        originalLifeTextPosition = playerLifeText.transform.position;
        
    }

    private void HandleNodeHitSuccess()
    {
        comboCount++;
        UpdateComboText();
        StopCoroutine(AnimateComboText()); // stop any existing animation
        StartCoroutine(AnimateComboText()); // start new animation
    }
    
    private IEnumerator AnimateComboText()
    {
        float duration = 0.5f; // total animation time
        float elapsedTime = 0f;

        // animation start (upward movement and color change to red)
        while (elapsedTime < duration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (duration / 2f);
            comboText.transform.position = new Vector3(originalPosition.x, Mathf.Lerp(originalPosition.y, originalPosition.y + 10f, t), originalPosition.z);
            comboText.color = Color.Lerp(originalColor, Color.red, t);
            yield return null;
        }

        elapsedTime = 0f;

        // animation start (downward movement and color change to white)
        while (elapsedTime < duration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (duration / 2f);
            comboText.transform.position = new Vector3(originalPosition.x, Mathf.Lerp(originalPosition.y + 10f, originalPosition.y, t), originalPosition.z);
            comboText.color = Color.Lerp(Color.red, originalColor, t);
            yield return null;
        }

        // at the end, set the position and color back to the original
        comboText.transform.position = originalPosition;
        comboText.color = originalColor;
    }
    
    private void HandleNodeHitFail()
    {
        comboCount = 0;
        UpdateComboText();
    }

    private void UpdateComboText()
    {
        comboText.text = "Combo: " + comboCount.ToString();
    }

    private void HandleSceneMonsterSpawned(EnemyBase spawnedEnemy)
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalculateDamage(int attackDamage)
    {
        playerLife -= attackDamage;
        playerLifeText.text = "x : " + playerLife.ToString();
        StopCoroutine(AnimateLifeText());
        StartCoroutine(AnimateLifeText());
    }
    
    private IEnumerator AnimateLifeText()
    {
        float duration = 0.5f; // total animation time
        float elapsedTime = 0f;

        // animation start (move from left to right)
        while (elapsedTime < duration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (duration / 2f);
            playerLifeText.transform.position = new Vector3(Mathf.Lerp(originalLifeTextPosition.x - 10f, originalLifeTextPosition.x, t), originalLifeTextPosition.y, originalLifeTextPosition.z);
            yield return null;
        }

        elapsedTime = 0f;

        // animation start (move from right to left)
        while (elapsedTime < duration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (duration / 2f);
            playerLifeText.transform.position = new Vector3(Mathf.Lerp(originalLifeTextPosition.x, originalLifeTextPosition.x - 10f, t), originalLifeTextPosition.y, originalLifeTextPosition.z);
            yield return null;
        }

        // at the end, set the position back to the original
        playerLifeText.transform.position = originalLifeTextPosition;
    }
}
