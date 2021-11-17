using System;
using System.Collections;
using System.Collections.Generic;
using EasyMobile;
using UnityEngine;
using UnityEngine.UI;

public class ThemeObject : MonoBehaviour
{
    public delegate void ChangeTheme(string themeName, Theme theme);
    public ChangeTheme changeTheme;
    private Theme theme;
    public bool Available { get; private set; } = true;
    [SerializeField]private Image image;
    [SerializeField]private Button button;

    public void ChangeThemeClick()
    {
        changeTheme(name, theme);
    }

    public void ChangeButtonColor()
    {
        image.color = Available ? button.colors.selectedColor : button.colors.highlightedColor;
    }

    public void SetUnSelected()
    {
        if (Available)
        {
            image.color = Color.white;
        }
        else
        {
            NotAvailable();
        }
    }

    public void NotAvailable()
    {
        Available = false;
        image.color = button.colors.disabledColor;
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
        SetUnSelected();
        
    }
}
