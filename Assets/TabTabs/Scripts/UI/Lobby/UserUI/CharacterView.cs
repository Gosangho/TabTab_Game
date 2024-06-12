using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterView : MonoBehaviour
{
    Button characterButton;
    public Image characterImage;
    public TextMeshProUGUI characterStory;

    public GameObject characterView;
    public Sprite[] characterSprite;
    // Start is called before the first frame update

    
    public void characterViewOpen(string characterName)
    {
        characterView.SetActive(true);

        if("Rana".Equals(characterName)) {
            characterImage.sprite = characterSprite[0];
            characterStory.text = "라나는 수줍음 많고 조용한 모습으로 마을 사람들 사이에서는 그녀의 존재를 쉽게 눈치챌 수 없습니다.\n마을을 지키는 경비대장의 딸로서 자랑스러운 경비단원이 되고 싶어합니다.\n수련에 대한 게으름이 전혀 없는 라나는 모험에 대한 열정과 마을을 지키는 의무를 함께 갖추고 있습니다.\n자신의 목표를 이루기 위해 땀 흘리며 훈련을 거듭하고, 어떤 상황에서도 포기하지 않습니다.";
        } else if("Sia".Equals(characterName)) {
            characterImage.sprite = characterSprite[1];
            characterStory.text = "왕족 출신의 천재 검술사.\n도도하며 자존심이 강한 그녀는 전투에서만큼은 항상 최고의 자리에 오르기 위해 노력합니다.\n냉철한 판단력과 전략적인 사고로 전투에서 성과를 보여주지만 이로인해 희생이 따르기도…\n그래서 그녀를 싫어하는 사람들도 있지만 시아는 오히려 더 강한 모습을 보이려 노력합니다.";
        } else if("Leon".Equals(characterName)) {
            characterImage.sprite = characterSprite[2];
            characterStory.text = "왕국의 파견기사 레온은 능란한 검술과 충직한 마음가짐으로 알려진 용맹한 기사 입니다.\n레온은 왕국의 명예를 지키고 평화를 유지하기 위해 항상 전장에 서 있는 용맹한 기사입니다.\n그의 검술은 뛰어나며 왕국을 수호하기 위해 싸웁니다.\n레온은 또한 충직한 마음가짐을 지니고 있습니다.\n그는 왕과 왕국에 대한 충성심이 깊으며, 언제나 그들을 지키기 위해 헌신합니다. 그의 헌신과 뛰어난 실력은 다른 기사들에게 영감을 주며, 그의 존재 자체가 왕국의 힘이 되고 있습니다.";
        } else if("Jena".Equals(characterName)) {
            characterImage.sprite = characterSprite[3];
            characterStory.text = "마을에서 태어나고 자란 평번한 소녀였지만 어느 날 나타난 괴물들에게 가족들을 잃고 맙니다.\n이 비극적인 사건으로 제나는 복수를 다짐하고, 자신을 강하게 만들기 위해 훈련을 시작합니다.\n그러는 와중 괴물의 습격으로부터 한 아이를 구하게 되고 제나는 복수보다는 다른 사람들을 지키기로 마음 먹고 증오만 남아있던 마음의 상처는 천천히 아물기 시작합니다.";
        }
    }

    public void characterViewClose()
    {
        characterView.SetActive(false);
    }
}
