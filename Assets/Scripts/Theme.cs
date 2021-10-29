using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "ScriptableObjects/Theme", order = 1)]
public class Theme : ScriptableObject
{
    public Color ballColor = new Color(114,156,255);
    public Color survivalBgColor = new Color(169,180,198);
    public Sprite classicBg;
    public Color machine;
}
