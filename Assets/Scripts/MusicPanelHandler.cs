using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicPanelHandler : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider, musicSlider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioMixer masterMixer;

    private void Start()
    {
        float sfxVolume, musicVolume;
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        masterMixer.SetFloat("sfxVolume", sfxVolume);
        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = sfxVolume;
        musicSlider.value = musicVolume;
        audioSource.Stop();
    }
    public void PlaySfxSample()
    {

        audioSource.Play();


    }
    public void SetSfxLevel(float sfxLevel)
    {
        masterMixer.SetFloat("sfxVolume", sfxLevel);
        PlayerPrefs.SetFloat("sfxVolume", sfxLevel);
    }
    public void SetMusicLevel(float musicLevel)
    {
        masterMixer.SetFloat("musicVolume", musicLevel);
        PlayerPrefs.SetFloat("musicVolume", musicLevel);
    }
}
