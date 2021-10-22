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
    [SerializeField] private GameObject survivalPool;
    [SerializeField] private SurvivalBGHandler survivalBgHandler;
    private void ChangeScreenBG()
    {
        baseImage.sprite = newBG;
        baseImage.color = color;
        DeleteClassicHoles();
        EnableSurvivalHoles();
        survivalBgHandler.ChangeToSurvivalBackground(color);
    }

    private void EnableSurvivalHoles()
    {
        foreach (Transform child in survivalPool.transform)
        {
            child.gameObject.SetActive(true);
        }
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
