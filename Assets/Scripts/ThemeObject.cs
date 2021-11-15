using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeObject : MonoBehaviour
{
    public delegate void ChangeTheme(string themeName, Theme theme);
    public ChangeTheme changeTheme;
    private Theme theme;
    public bool Available { get; private set; } = true;


    public void ChangeThemeClick()
    {
        changeTheme(name, theme);
    }

    public void ChangeButtonColor()
    {
        GetComponent<Image>().color = GetComponent<Button>().colors.selectedColor;
    }

    public void RemoveSelected()
    {
        GetComponent<Image>().color = Color.white;
    }

    public void NotAvailable()
    {
        Available = false;
        GetComponent<Image>().color = GetComponent<Button>().colors.disabledColor;
    }

    public void SetTheme(Theme theme)
    {
        this.theme = theme;
    }

    public Theme GetTheme()
    {
        return theme;
    }
    public void UnlockTheme()
    {
        Available = true;
    }
}
