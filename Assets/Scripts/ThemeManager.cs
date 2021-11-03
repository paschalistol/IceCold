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
            if (!theme.available)
            {
                themeInstance.GetComponent<Button>().interactable = false;
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
    }

    private void ChangeTheme(string name)
    {
        PlayerPrefs.SetString("startingTheme", name);
        GetStartingTheme();
    }
}
