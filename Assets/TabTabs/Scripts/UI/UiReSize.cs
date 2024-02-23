using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiReSize : MonoBehaviour
{
    private RectTransform rectTransform;
    private float baseWidth = 540;
    private float baseHeight = 860;

    [SerializeField] public GameObject GameSetting;
    [SerializeField] public GameObject UserUI;
    [SerializeField] public GameObject StoreUI;
    [SerializeField] public GameObject RankingUI;
    [SerializeField] public GameObject AttendanceUI;


    // Start is called before the first frame update
    void Awake() {

        GameSetting.SetActive(true);
        UserUI.SetActive(true);
        StoreUI.SetActive(true);
        RankingUI.SetActive(true);
        AttendanceUI.SetActive(true);
        
        GameObject[] dynamicLayers = GameObject.FindGameObjectsWithTag("DynamicLayer");
        GameObject[] dynamicImages = GameObject.FindGameObjectsWithTag("DynamicImage");
        GameObject[] dynamicSpine = GameObject.FindGameObjectsWithTag("DynamicSpine");
        GameObject[] dynamicGrid = GameObject.FindGameObjectsWithTag("DynamicGrid");
        GameObject[] dynamicFont = GameObject.FindGameObjectsWithTag("DynamicFont");

        foreach (GameObject obj in dynamicLayers)
        {
            BaseCanvsWidth(obj);
        }

        foreach (GameObject obj in dynamicImages)
        {
            Debug.Log(obj.name);
            FitImageAspectRatio(obj);
        }

        foreach (GameObject obj in dynamicSpine)
        {
            Debug.Log(obj.name);
            FitSprineAspectRatio(obj);
        }

        foreach (GameObject obj in dynamicGrid)
        {
            Debug.Log(obj.name);
            FitGridSize(obj);
        }

        foreach (GameObject obj in dynamicFont)
        {
            FitFontSize(obj);
        }

        GameSetting.SetActive(false);
        UserUI.SetActive(false);
        StoreUI.SetActive(false);
        RankingUI.SetActive(false);
        AttendanceUI.SetActive(false);

    }



    void BaseCanvsWidth(GameObject objectName) {
        RectTransform canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();;
        rectTransform = objectName.GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2((float)canvasRect.rect.width, rectTransform.sizeDelta.y);
    }

    void FitFontSize(GameObject objectName) {
    /*    TextMeshProUGUI text = objectName.GetComponent<TextMeshProUGUI>();
        float rateWidth = (float)Screen.width / baseWidth;
        float fontSize = (float)rateWidth * text.fontSize;
        text.fontSize = (int)fontSize;
        FitImageAspectRatio(objectName);*/
    }

    void FitImageAspectRatio(GameObject objectName)
    {
        RectTransform canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        rectTransform = objectName.GetComponent<RectTransform>();

        float rateWidth = (float)canvasRect.rect.width / baseWidth;
        float rateHeight = (float)canvasRect.rect.height / baseHeight;

        float scale = Mathf.Min(rateWidth, rateHeight);

        float imageX = (float)scale * rectTransform.anchoredPosition.x;
        float imageY = (float)scale * rectTransform.anchoredPosition.y;

        rectTransform.anchoredPosition = new Vector2(imageX, imageY);

        if(canvasRect.rect.width < 540) {
            float imageWidth = (float)scale * rectTransform.sizeDelta.x;
            float imageHeight = (float)scale * rectTransform.sizeDelta.y;

            rectTransform.sizeDelta = new Vector2(imageWidth, imageHeight);
            
        }
    }

    void FitSprineAspectRatio(GameObject objectName)
    {
        RectTransform canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        if(canvasRect.rect.width < 400) {
            rectTransform = objectName.GetComponent<RectTransform>();

            rectTransform.localScale = new Vector3(2.5f, 2.5f, 1f);
        }
    }

    void FitGridSize(GameObject objectName) {
        RectTransform canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        rectTransform = objectName.GetComponent<RectTransform>();

        float rateWidth = (float)canvasRect.rect.width / baseWidth;
        float rateHeight = (float)canvasRect.rect.height / baseHeight;

        float scale = Mathf.Min(rateWidth, rateHeight);

        float imageX = (float)scale * rectTransform.anchoredPosition.x;
        float imageY = (float)scale * rectTransform.anchoredPosition.y;

        float imageWidth = (float)scale * rectTransform.sizeDelta.x;
        float imageHeight = (float)scale * rectTransform.sizeDelta.y;

        rectTransform.sizeDelta = new Vector2(imageWidth, imageHeight);

        GridLayoutGroup _gridLayoutGroup = objectName.GetComponent<GridLayoutGroup>();

        float rateWidths = (float)canvasRect.rect.width / baseWidth;
        float rateHeights = (float)canvasRect.rect.height / baseHeight;

        float gridWidths = (float)rateWidths * _gridLayoutGroup.cellSize.x;
        float gridHeights = (float)rateHeights * _gridLayoutGroup.cellSize.y;

        float gridSpaceX = (float)rateWidths * _gridLayoutGroup.spacing.x;
        float gridSpaceY = (float)rateHeights * _gridLayoutGroup.spacing.y;

        _gridLayoutGroup.cellSize = new Vector2(gridWidths, gridHeights);
        _gridLayoutGroup.spacing = new Vector2(gridSpaceX, gridSpaceY);
        _gridLayoutGroup.padding.top = (int)Math.Ceiling(rateHeights *_gridLayoutGroup.padding.top);
    }

}
