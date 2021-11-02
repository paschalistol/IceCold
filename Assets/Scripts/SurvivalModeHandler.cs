using System;
using System.Collections;
using System.Collections.Generic;
using EasyMobile;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SurvivalModeHandler : MonoBehaviour
{
    [SerializeField] private Sprite newBG;
    [SerializeField] private Color color;
    [SerializeField] private Image baseImage;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject survivalPool;
    [SerializeField] private SurvivalBGHandler survivalBgHandler;
    [SerializeField] private float startingTime = 10.0f;
    private float timeLeft = 10.0f;
    [SerializeField] private TMP_Text bonusPointScreen;
    [SerializeField] private int secondsToAdd = 15;

    private void Start()
    {
        ClassicGameManager.instance.startRound += StartRound;
    }

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
            if (child.transform.CompareTag("Hole"))
            {
                child.gameObject.SetActive(true);
            }
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

    private void StartRound()
    {
        timeLeft = startingTime;
    }

    public void AddTime()
    {
        timeLeft += secondsToAdd;
    }
    private void Update()
    {
        if (ClassicGameManager.instance.GetAllowControls())
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                bonusPointScreen.SetText(((int)timeLeft).ToString());
            }
            else if (timeLeft <= 0)
            {
                bonusPointScreen.SetText("0");
                ClassicGameManager.instance.Die();
            }
        }

    }
}
