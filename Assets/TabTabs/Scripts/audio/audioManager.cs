using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class audioManager : MonoBehaviour
{
    [SerializeField]private AudioSource BgmAudio;
    [SerializeField] private AudioClip[] BgmClip;
    [SerializeField] public Slider BgmSetSlider;
    [SerializeField]private AudioSource SfxAudio;
    [SerializeField] private AudioClip[] SfxClip;
    [SerializeField] public Slider SfxSetSlider;

    void Start()
    {
        BgmAudio = GetComponent<AudioSource>();
        BgmAudio.volume = BgmSetSlider.value;
        SfxAudio = GetComponent<AudioSource>();
        SfxAudio.volume = SfxSetSlider.value;

        int ranAudio = Random.Range(0, 3);
        BgmAudio.clip = BgmClip[ranAudio];
        BgmAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        BgmAudio.volume = BgmSetSlider.value;

        if (!BgmAudio.isPlaying)
        {
            int ranAudio = Random.Range(0, 3);
            BgmAudio.clip = BgmClip[ranAudio];
            BgmAudio.Play();
        }
    }
}
