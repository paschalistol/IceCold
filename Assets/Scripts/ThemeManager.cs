using System.Collections.Generic;
using EasyMobile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    [SerializeField] private Theme[] themes;
    private string startingThemeName;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject themePrefab;
    [SerializeField] private GameObject themePopUp;
    [SerializeField] private TMP_Text themePopUpCloseButton;

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
        // ball.EnableKeyword("_EMISSION");
        // bar.EnableKeyword("_EMISSION");
        
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

    private void ChangeTheme(string themeName)
    {
        Theme tempTheme = null;
        foreach (Theme theme in themes)
        {
            if (theme.name.Equals(themeName))
            {
                tempTheme = theme;
                break;
            }
        }

        if (tempTheme != null)
        {
            if (tempTheme.available)
            {
                RemoveOldSelected();
                PlayerPrefs.SetString("startingTheme", themeName);
                GetStartingTheme();
                ChangeButtonColor(themeName);
            }
            else
            {
                ShowLockedPopUp(tempTheme.themeReasonLocked);
            }
        }
  
    }

    private void ShowLockedPopUp(Theme.ThemeReasonLocked tempThemeThemeReasonLocked)
    {
        switch (tempThemeThemeReasonLocked)
        {
            case Theme.ThemeReasonLocked.Achievement:
                break;
            case Theme.ThemeReasonLocked.IAP:
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
}
