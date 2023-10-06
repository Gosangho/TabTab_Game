using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TabTabs.NamChanwoo
{
    public class Left_Orc2_Material : MonoBehaviour
    {
        public Material newMaterial;
        public float Left_brightnessValue1; // ���� ��ü�� ��ϰ�
        Test3Battle Test3BattleInstance1;
        private void Start()
        {
            Test3BattleInstance1 = FindObjectOfType<Test3Battle>();
            StartCoroutine(ChangeMaterialAfterOneFrame());
        }

        private IEnumerator ChangeMaterialAfterOneFrame()
        {
            yield return null;

            // Material ���� �ڵ�
            GetComponent<Renderer>().material = newMaterial;
            newMaterial.SetFloat("_Brightness", Left_brightnessValue1);
        }

        private void Update()
        {
            if (Test3BattleInstance1.selectEnemy == Test3BattleInstance1.LeftEnemy)
            {
                Left_brightnessValue1 = 1.0f;
            }
            else
            {
                Left_brightnessValue1 = 0.2f;
            }

            newMaterial.SetFloat("_Brightness", Left_brightnessValue1);
        }
        // Material�� ���� ���������� ������ �ȵŴ� ���� -> start�� awake���� �����ε���.
        // ������ runtime�߿��� ������ ������
        // �ذ��ؾ���
        // �̸� �̿��ؼ� �ӽ÷� runtime�߿� �� ������ ����Ŀ� mesh renderer�� material�� ������ material�� ����
        // -> �̸����� �� ��ü�� ������ ��ϰ��� ���� �� �ִ�.
        // but ���� �������� �����ؼ� �ϴ� ����� �ذ��غ���.
    }
}





