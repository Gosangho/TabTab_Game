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
        } else if("Sia".Equals(characterName)) {
            characterImage.sprite = characterSprite[1];
        } else if("Leon".Equals(characterName)) {
            characterImage.sprite = characterSprite[2];
        } else if("Jena".Equals(characterName)) {
            characterImage.sprite = null; 
        }
    }

    public void characterViewClose()
    {
        characterView.SetActive(false);
    }
}
