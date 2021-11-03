using System.Collections.Generic;
using EasyMobile;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    [SerializeField] private Theme[] themes;
    private string startingThemeName;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject themePrefab;

    [Header("Materials and Sprites")] 
    [SerializeField] private Image classicScreen;

    [SerializeField] private Material ball;
    [SerializeField] private Material bar;
    [SerializeField] private Material machineMain;
    [SerializeField] private Material machineSecondary;
    private Dictionary<string, GameObject> buttons = new Dictionary<string, GameObject>();
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
            themeInstance.GetComponent<ThemeObject>().changeTheme += ChangeTheme;
            themeInstance.name = theme.name;
            buttons.Add(themeInstance.name, themeInstance);
            if (!theme.available)
            {
                themeInstance.GetComponent<Button>().interactable = false;
                themeInstance.GetComponent<ThemeObject>().NotAvailable();
            }
            else if (theme.name.Equals(ActiveTheme.name))
            {
                themeInstance.GetComponent<ThemeObject>().ChangeButtonColor();
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

    private void ChangeTheme(string name)
    {
        RemoveOldSelected();
        PlayerPrefs.SetString("startingTheme", name);
        GetStartingTheme();
    }

    private void RemoveOldSelected()
    {
        foreach (KeyValuePair<string,GameObject> theme in buttons)
        {
            if (theme.Key.Equals(ActiveTheme.name))
            {
                theme.Value.GetComponent<ThemeObject>().RemoveSelected();
            }
        }

    }
}
