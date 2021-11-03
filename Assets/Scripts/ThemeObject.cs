using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeObject : MonoBehaviour
{
    public delegate void ChangeTheme(string name);
    public ChangeTheme changeTheme;
    

    public void ChangeThemeClick()
    {
        changeTheme(name);
    }
}
