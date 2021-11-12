using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeObject : MonoBehaviour
{
    public delegate void ChangeTheme(string name);
    public ChangeTheme changeTheme;
    private bool available = true;
    public bool Available
    {
        get => available;
        private set => available = value;
    }
    

    public void ChangeThemeClick()
    {
        changeTheme(name);
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
}
