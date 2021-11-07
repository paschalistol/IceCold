using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemePopUpAnimation : MonoBehaviour
{
    [SerializeField] private GameObject ThemePopUp;

    public void ClosePopUp()
    {
        ThemePopUp.SetActive(false);
    }
}
