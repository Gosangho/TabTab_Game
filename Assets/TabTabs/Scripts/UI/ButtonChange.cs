using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChange : MonoBehaviour
{
    public Button AttackButton;
    public Button DashButton;
    public Transform AttackButtonTrans;
    public Transform DashButtonTrans;

    public Sprite controlTypeA;
    public Sprite controlTypeB;

    string controlType;
    
    void Start()
    {
        controlType = DataManager.Instance.getCharacter(1);

        if( AttackButton != null || DashButton != null)
        {
            AttackButtonTrans = AttackButton.transform;
            DashButtonTrans = DashButton.transform;
        }

        Debug.Log("controlType : " + controlType);
        Image image = GetComponent<Image>();
        if("B".Equals(controlType))
        {
            if (image != null)
            {
                image.sprite = controlTypeB;
            }
        } else {
            if (image != null)
            {
                image.sprite = controlTypeA;
            }
        }
    }

    private void ButtonChangeTrans() {
        Image image = GetComponent<Image>();
        if("B".Equals(controlType))
        {
            if (image != null)
            {
                image.sprite = controlTypeB;
            }
        } else {
            if (image != null)
            {
                image.sprite = controlTypeA;
            }
        }

   
        if( AttackButton != null || DashButton != null)
        {
            Vector3 tempPosition = AttackButtonTrans.position;
            AttackButtonTrans.position = DashButtonTrans.position;
            DashButtonTrans.position = tempPosition;
        }
    }

    public void ButtonTransform()
    {
        audioManager.Instance.SfxAudioPlay("Ui_Click");
          
        if("B".Equals(controlType))
        {
            controlType = "A";
            DataManager.Instance.selectCharacter(1, "A");
        } else {
            controlType = "B";
            DataManager.Instance.selectCharacter(1, "B");
        }

        ButtonChangeTrans();
    }
}
