using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TImebar : MonoBehaviour
{
    Image timebarImage;

    public Action TimeOver;
        
    void Start()
    {
        timebarImage = GetComponent<Image>();
    }

    void Update()
    {
        /*timebarImage.fillAmount -= Time.deltaTime * 0.1f;*/
       
        if (timebarImage.fillAmount<=0)
        {
            TimeOver();
            timebarImage.fillAmount = 1.0f;
        }
    }
}
