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
    
    void Start()
    {
        AttackButtonTrans = AttackButton.transform;
        DashButtonTrans = DashButton.transform;
    }

    public void ButtonTransform()
    {
        audioManager.Instance.SfxAudioPlay("Ui_Click");
        Vector3 tempPosition = AttackButtonTrans.position;
        AttackButtonTrans.position = DashButtonTrans.position;
        DashButtonTrans.position = tempPosition;
    }
}
