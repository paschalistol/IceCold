using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;

[CreateAssetMenu(fileName = "Theme", menuName = "ScriptableObjects/Theme", order = 1)]
public class Theme : ScriptableObject
{
    public Color ballColor = new Color(114/255f,156/255f,255/255f);
    public Color survivalBgColor = new Color(169/255f,180/255f,198/255f);
    public Sprite classicBg;
    public Color machineMain;
    public Color machineSecondary;
    public Color bar = new Color(207/255f, 77/255f, 0);
    public Sprite platform;
    public bool available;
    public ThemeReasonLocked themeReasonLocked;
    [Tooltip("To unlock this theme you'll have to "), TextArea]public string achievementDescription;
    public string achievementID;
    public enum ThemeReasonLocked
    {
        IAP, Achievement
    }
}
