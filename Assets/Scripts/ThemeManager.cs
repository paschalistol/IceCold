using System;
using System.Collections.Generic;
using EasyMobile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

public class ThemeManager : MonoBehaviour
{
    [SerializeField] private Theme[] themes;
    private string startingThemeName;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject themePrefab;
    
    [Header("Theme Pop Up")] 
    [SerializeField] private GameObject themePopUp;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private TMP_Text themePopUpCloseButton;
    [SerializeField] private TMP_Text themePopUpText;
    [SerializeField] private string defaultUnlockText = "To unlock this theme you'll have to ";
    
    [Header("Materials and Sprites")] 
    [SerializeField] private Image classicScreen;

    [SerializeField] private Material ball;
    [SerializeField] private Material bar;
    [SerializeField] private Material machineMain;
    [SerializeField] private Material machineSecondary;
    private Dictionary<string, GameObject> buttons = new Dictionary<string, GameObject>();
    private Dictionary<string, ThemeObject> themeObjects = new Dictionary<string, ThemeObject>();
    public Theme ActiveTheme
    {
        get;
        private set;
    }

    private void Awake()
    {
        GetStartingTheme();
    }

    private void Start()
    {
        foreach(Theme theme in themes)
        {
            GameObject themeInstance = Instantiate(themePrefab, parent.transform);
            themeInstance.transform.GetChild(0).GetComponent<Image>().sprite = theme.platform;
            ThemeObject themeObject = themeInstance.GetComponent<ThemeObject>();
            themeObject.changeTheme += ChangeTheme;
            themeInstance.name = theme.name;
            themeObject.SetTheme(theme);
            buttons.Add(themeInstance.name, themeInstance);
            themeObjects.Add(themeInstance.name, themeObject);
            if (!theme.available)
            {
                // themeInstance.GetComponent<Button>().interactable = false;
                themeObject.NotAvailable();
            }
            else if (theme.name.Equals(ActiveTheme.name))
            {
                themeObject.ChangeButtonColor();
            }
        }
        GameServices.UserLoginSucceeded += UnlockThemes;
        ChangeChildrenOrder();
        // ball.EnableKeyword("_EMISSION");
        // bar.EnableKeyword("_EMISSION");
    }

    private void OnDestroy()
    {
        GameServices.UserLoginSucceeded -= UnlockThemes;
    }

    private void UnlockThemes()
    {
        if (GameServices.IsInitialized())
        {
            Social.LoadAchievements(achievements =>
            {
                if (achievements.Length > 0)
                {
                    foreach (IAchievement achievement in achievements)
                    {
                        if (!achievement.completed) continue;
                        foreach (Theme theme in themes)
                        {
                            if (theme.achievementID.Equals(achievement.id))
                            {
                                UnlockTheme(theme.name);
                            }
                        }
                    }
                }
                else
                    Debug.Log("No achievements returned");
            });
        }
  
    }

    private void UnlockTheme(string themeName)
    {
        foreach (KeyValuePair<string,ThemeObject> theme in themeObjects)
        {
            if (theme.Key.Equals(themeName))
            {
                theme.Value.UnlockTheme();
            }
        }
    }

    private void GetStartingTheme()
    {
        startingThemeName = PlayerPrefs.GetString("startingTheme", "Default Theme");
        foreach (Theme theme in themes)
        {
            if (theme.name.Equals(startingThemeName))
            {
                ActiveTheme = theme;
                ChangeMaterialsAndSprites();
            }
        }
    }

    private void ChangeMaterialsAndSprites()
    {
        classicScreen.sprite = ActiveTheme.classicBg;
        ball.SetColor("_Emission", ActiveTheme.ballColor);
        bar.SetColor("_Emission", ActiveTheme.bar);
        machineMain.SetColor("_Color", ActiveTheme.machineMain);
        machineSecondary.SetColor("_EmissionColor", ActiveTheme.machineSecondary);
    }

    private void ChangeTheme(string themeName, Theme theme)
    {
        ThemeObject tempObject = null;
        foreach (KeyValuePair<string,ThemeObject> themeObject in themeObjects)
        {
            if (themeObject.Key.Equals(themeName))
            {
                tempObject = themeObject.Value;
            }
        }
            if (tempObject != null)
            {
                
            if (tempObject.gameObject.name.Equals(themeName) && tempObject.Available)
            {
                RemoveOldSelected();
                tempObject.RemoveSelected();
                PlayerPrefs.SetString("startingTheme", themeName);
                GetStartingTheme();
                ChangeButtonColor(themeName);
            }
            else
            {
                ShowLockedPopUp(theme.themeReasonLocked, theme);
            }
            }
    }

    private void ShowLockedPopUp(Theme.ThemeReasonLocked tempThemeThemeReasonLocked, Theme tempTheme)
    {
        themePopUp.SetActive(true);
        switch (tempThemeThemeReasonLocked)
        {
            case Theme.ThemeReasonLocked.Achievement:
                themePopUpText.text = defaultUnlockText + tempTheme.achievementDescription;
                themePopUpCloseButton.text = "OK";
                buyButton.SetActive(false);
                break;
            case Theme.ThemeReasonLocked.IAP:
                themePopUpText.text = "The theme will be available soon";
                // themePopUpCloseButton.text = "Cancel";
                // buyButton.SetActive(true);
                break;
        }
    }

    private void ChangeButtonColor(string themeName)
    {
        foreach (KeyValuePair<string,ThemeObject> theme in themeObjects)
        {
            if (theme.Key.Equals(themeName))
            {
                theme.Value.ChangeButtonColor();
            }
        }
    }

    private void RemoveOldSelected()
    {
        foreach (KeyValuePair<string,ThemeObject> theme in themeObjects)
        {
            if (theme.Key.Equals(ActiveTheme.name))
            {
                theme.Value.RemoveSelected();
            }
        }
    }

    private void ChangeChildrenOrder()
    {
        int currentSiblingIndex = 0;
        foreach (KeyValuePair<string,ThemeObject> theme in themeObjects)
        {
            if (theme.Value.Available)
            {
                theme.Value.gameObject.transform.SetSiblingIndex(currentSiblingIndex);
                currentSiblingIndex++;
            }
        }
    }
}
