using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TabTabs.NamChanwoo {
    public class Right_Orc2_material : MonoBehaviour
    {
        public Material objectMaterial;
        public float Right_brightnessValue; // 개별 객체의 명암값
        Test3Battle Test3BattleInstance;
        private void Start()
        {
            // 새로운 Material 인스턴스를 생성하여 현재 객체의 Material을 설정합니다.
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




