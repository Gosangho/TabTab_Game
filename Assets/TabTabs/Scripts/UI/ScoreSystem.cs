using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI ScoreText;
    public int Score;
    void Start()
    {
        ScoreText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        ScoreText.text = "Score : " + Score.ToString();
    }
}
