using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;
    private bool pause = false;

    public void SetSfxLevel(float sfxLevel)
    {
        masterMixer.SetFloat("sfxVolume",sfxLevel);
    }
    public void SetMusicLevel(float musicLevel)
    {
        masterMixer.SetFloat("musicVolume", musicLevel);
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void PauseMenu()
    {
        pause = !pause;
        if (pause)
        {
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
}
