using System;
using EasyMobile;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Options : MonoBehaviour
{
    [SerializeField] private GameObject soundPanel, pausePanel;
    private bool pause = false;
    [SerializeField] private RectTransform machinePanel, screenPanel, optionsBackground, fade, themeSelector;
    [SerializeField] private Vector3 fullScreenPosition, fullScreenScale;
    private bool fullScreen;
    [SerializeField] private TMP_Text signText;
    public delegate void TogglingFullscreen();
    public TogglingFullscreen toggleFullscreen;
    private bool signedIn = false;
    private void Start()
    {
        fullScreen = (PlayerPrefs.GetInt("fullScreen") != 0);
        //FullScreen();
        //OptionsBackground();
        ChangeFullScreenOptions();
        InitSignText();
    }

    private void InitSignText()
    {
        // signedIn = user signed in
        if (signedIn)
        {
            signText.text = "Sign out";
        }
        else
        {
            signText.text = "Sign in";
        }


    }

    public void SignInButton()
    {
        if (signedIn)
        {
            GameServices.SignOut();
        }
        else
        {
            //try sign in
        }
        InitSignText();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowLeaderboard()
    {
        GameServices.ShowLeaderboardUI(EM_GameServicesConstants.Leaderboard_Survival_High_Score);
    }

    public void SignIn()
    {
        
    }
    public void SignOut()
    {
        
    }
    
    public void Quit()
    {
        Application.Quit();
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
        //OptionsBackground();
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
            optionsBackground.sizeDelta = new Vector2(0,1190);
        }
        else
        {
            optionsBackground.anchorMin = new Vector2(0.09f, 0);
            optionsBackground.anchorMax = new Vector2(0.91f, 0);
            optionsBackground.anchoredPosition = new Vector3(0, 380, -80);
            optionsBackground.sizeDelta = new Vector2(0, 976);
        }
    }
    /// <summary>
    ///Changes the anchors of UI objects
    /// also important to add the panel at CameraScript.scalers
    /// </summary>
    private void FullScreen()
    {
        if (fullScreen)
        {
            machinePanel.anchoredPosition = fullScreenPosition;
            machinePanel.localScale = fullScreenScale;
            screenPanel.anchoredPosition = fullScreenPosition;
            screenPanel.localScale = fullScreenScale;
            fade.anchoredPosition = fullScreenPosition;
            fade.localScale = fullScreenScale;
            optionsBackground.anchoredPosition = fullScreenPosition;
            optionsBackground.localScale = fullScreenScale;
            themeSelector.anchoredPosition = fullScreenPosition;
            themeSelector.localScale = fullScreenScale;
        }
        else
        {
            machinePanel.anchoredPosition = Vector3.zero;
            machinePanel.localScale = Vector3.one;
            screenPanel.anchoredPosition = Vector3.zero;
            screenPanel.localScale = Vector3.one;
            fade.anchoredPosition = Vector3.zero;
            fade.localScale = Vector3.one;
            optionsBackground.anchoredPosition = Vector3.zero;
            optionsBackground.localScale = Vector3.one;
            themeSelector.anchoredPosition = Vector3.zero;
            themeSelector.localScale = Vector3.one;
        }
    }
}
