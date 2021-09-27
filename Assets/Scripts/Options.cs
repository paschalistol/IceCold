using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Options : MonoBehaviour
{
    [SerializeField] private GameObject soundPanel, pausePanel;
    private bool pause = false;
    [SerializeField] private RectTransform machinePanel, screenPanel, optionsBackground;
    [SerializeField] private Vector3 fullScreenPosition, fullScreenScale;
    private bool fullScreen;
    public delegate void TogglingFullscreen();
    public TogglingFullscreen toggleFullscreen;
    private void Start()
    {
        fullScreen = (PlayerPrefs.GetInt("fullScreen") != 0);
        //FullScreen();
        OptionsBackground();
        ChangeFullScreenOptions();
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseMenu()
    {
        var sources = FindObjectsOfType<AudioSource>();

        pause = !pause;
        pausePanel.SetActive(pause);
        soundPanel.SetActive(pause);
        optionsBackground.gameObject.SetActive(pause);
        if (pause)
        {
            Time.timeScale = 0;
 
            foreach (var source in sources)
            {
                if (!source.outputAudioMixerGroup.name.Equals("Music"))
                {
                    source.Pause();
                }
            }
        }
        else
        {
            Time.timeScale = 1;
            foreach (var source in sources)
            {
                if (!source.outputAudioMixerGroup.ToString().Equals("Music"))
                {
                    source.UnPause();
                }
            }
        }
    }
    private void ChangeFullScreenOptions()
    {
        FullScreen();
        OptionsBackground();
        toggleFullscreen();
    }
    public void ToggleFullscreen()
    {
        fullScreen = !fullScreen;
        PlayerPrefs.SetInt("fullScreen", (fullScreen ? 1 : 0));
        ChangeFullScreenOptions();
    }

    private void OptionsBackground()
    {
        if (fullScreen)
        {
            optionsBackground.anchorMin = Vector2.zero;
            optionsBackground.anchorMax = new Vector2(1,0);
            optionsBackground.anchoredPosition = new Vector3(0, 275, -80);
            optionsBackground.sizeDelta = new Vector2(0,1200);
        }
        else
        {
            optionsBackground.anchorMin = new Vector2(0.05f, 0);
            optionsBackground.anchorMax = new Vector2(0.95f, 0);
            optionsBackground.anchoredPosition = new Vector3(0, 380, -80);
            optionsBackground.sizeDelta = new Vector2(0, 982);
        }
    }

    private void FullScreen()
    {
        if (fullScreen)
        {
            machinePanel.anchoredPosition = fullScreenPosition;
            machinePanel.localScale = fullScreenScale;
            screenPanel.anchoredPosition = fullScreenPosition;
            screenPanel.localScale = fullScreenScale;
        }
        else
        {
            machinePanel.anchoredPosition = Vector3.zero;
            machinePanel.localScale = Vector3.one;
            screenPanel.anchoredPosition = Vector3.zero;
            screenPanel.localScale = Vector3.one;
        }
    }
}
