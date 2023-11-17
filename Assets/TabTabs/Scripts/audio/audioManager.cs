using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class audioManager : MonoBehaviour
{
    [SerializeField]private AudioSource BgmAudio;
    [SerializeField] private AudioClip[] BgmClip;
    [SerializeField] public Slider BgmSetSlider;
    [SerializeField]private AudioSource SfxAudio_Char_AttackAudio;
    [SerializeField]public AudioSource SfxAudio_Enemy_hitAudio;
    [SerializeField] private AudioClip[] SfxClip;
    [SerializeField] private AudioClip[] SfxClip_Enemyhit;
    [SerializeField] public Slider SfxSetSlider;
    public static audioManager Instance;

    private void Awake()
    {
        Instance = this;
        BgmAudio = GetComponent<AudioSource>();
        SfxAudio_Char_AttackAudio = GameObject.Find("SFX_audio_CharAttack").GetComponent<AudioSource>();
        SfxAudio_Enemy_hitAudio = GameObject.Find("SFX_audio_Enemyhit").GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {// 배틀
            int ranBGM = Random.Range(0, 3);
            BgmAudio.clip = BgmClip[ranBGM];
            BgmAudio.Play();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {// 로비(루프)
            BgmAudio.Play();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {// 튜토리얼(루프)
            BgmAudio.Play();
        }
        else
        {// 엔딩(루프)
            BgmAudio.Play();
        }
    }
    private void Update()
    {
        BgmAudio.volume = BgmSetSlider.value;
        SfxAudio_Char_AttackAudio.volume = SfxSetSlider.value;
        SfxAudio_Enemy_hitAudio.volume = SfxSetSlider.value;
        if (!BgmAudio.isPlaying && SceneManager.GetActiveScene().buildIndex==3)
        {
            int ranBGM = Random.Range(0, 3);
            BgmAudio.clip = BgmClip[ranBGM];
            BgmAudio.Play();
        }

        if (!BgmAudio.isPlaying)
        {
            BgmAudio.Play();
        }
    }
    
    public void SfxAudioPlay(string type)
    {
        int Sfxindex = 0;

        switch (type)
        {
            case "Ui_Click": Sfxindex = 0; break;
            case "Char_Attack1": Sfxindex = 1; break;
            case "Char_Attack2": Sfxindex = 2; break;
            case "Char_Dash1": Sfxindex = 3; break;
            case "Char_Dash2": Sfxindex = 4; break;
            case "Char_Dead": Sfxindex = 5; break;
            case "Char_Spirit": Sfxindex = 6; break;
            case "ItemGet": Sfxindex = 7; break;
            case "Tutorial_Text": Sfxindex = 8; break;
        }
        
        SfxAudio_Char_AttackAudio.clip = SfxClip[Sfxindex];
        SfxAudio_Char_AttackAudio.Play();
    }
    public void SfxAudioPlay_Enemy(string type)
    {
        int Sfxindex = 0;

        switch (type)
        {
            
            case "Enemy_Dead": Sfxindex = 0; break;
            case "Enemy_Hit": Sfxindex = 1; break;
            case "Enemy_Attack": Sfxindex = 2; break;
            case "Tutorial_Warning": Sfxindex = 3; break;

        }

        SfxAudio_Enemy_hitAudio.clip = SfxClip_Enemyhit[Sfxindex];
        SfxAudio_Enemy_hitAudio.Play();
    }
}
