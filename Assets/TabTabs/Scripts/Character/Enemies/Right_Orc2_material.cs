using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TabTabs.NamChanwoo {
    public class Right_Orc2_material : MonoBehaviour
    {
        public Material objectMaterial;
        public float Right_brightnessValue; // ���� ��ü�� ��ϰ�
        Test3Battle Test3BattleInstance;
        private void Start()
        {
            // ���ο� Material �ν��Ͻ��� �����Ͽ� ���� ��ü�� Material�� �����մϴ�.
            Test3BattleInstance = FindObjectOfType<Test3Battle>();
            GetComponent<Renderer>().material = objectMaterial;
        }
        private void Update()
        {
            if (Test3BattleInstance.selectEnemy == Test3BattleInstance.RightEnemy)
            {
                Right_brightnessValue = 1.0f;
            }
            else
            {
                Right_brightnessValue = 0.2f;
            }

            objectMaterial.SetFloat("_Brightness", Right_brightnessValue);

        }
    }
}




