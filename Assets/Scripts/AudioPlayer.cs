using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : PickUpAudioPlayer
{
    protected override void OnEnable()
    {
        
    }

    public void PlayClip(AudioClip clip, AudioMixerGroup mixerGroup)
    {
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.Play();
        StartCoroutine(WaitAndDisable());
    }
}
