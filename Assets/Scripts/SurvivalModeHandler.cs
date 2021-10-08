using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalModeHandler : MonoBehaviour
{
    [SerializeField] private Sprite newBG;
    [SerializeField] private Color color;
    [SerializeField] private Image baseImage;
    [SerializeField] private GameObject gameManager;
    private void ChangeScreenBG()
    {
        baseImage.sprite = newBG;
        baseImage.color = color;
        DeleteClassicHoles();
    }

    private void DeleteClassicHoles()
    {
        foreach (var child in gameManager.GetComponentsInChildren<Transform>())
        {
            if (child != gameManager.transform)
            {
                Destroy(child.gameObject);
            }

        }
    }
}
