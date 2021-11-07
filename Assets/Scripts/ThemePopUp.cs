using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemePopUp : MonoBehaviour
{
    [SerializeField] private Animation anim;
    private void OnEnable()
    {
        anim.Play("OpenThemePopUp");
    }

    public void ClosePopUp()
    {
        anim.Play("CloseThemePopUp");
    }
}
