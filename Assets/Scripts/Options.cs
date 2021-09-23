using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;

    public void SetSfxLevel(float sfxLevel)
    {
        masterMixer.SetFloat("sfxVolume",sfxLevel);
    }
    public void SetMusicLevel(float musicLevel)
    {
        masterMixer.SetFloat("musicVolume", musicLevel);
    }
}
