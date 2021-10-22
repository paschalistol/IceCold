using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalBGHandler : MonoBehaviour
{
    [SerializeField] private RectTransform classicModeBackground, survivalTile1Panel, survivalTile2Panel;
    private Color bgColor;
    [SerializeField] private SpriteRenderer survivalTile1, survivalTile2;

    public void ChangeToSurvivalBackground(Color color)
    {
        classicModeBackground.gameObject.SetActive(false);
        survivalTile1Panel.gameObject.SetActive(true);
        survivalTile2Panel.gameObject.SetActive(true);
        bgColor = color;
        survivalTile1.color = bgColor;
        survivalTile2.color = bgColor;
    }
    private void Update()
    {
        if (survivalTile1Panel.anchoredPosition.y < -1600)
        {
            survivalTile1Panel.anchoredPosition =  new Vector2(0, survivalTile1Panel.anchoredPosition.y + 3200);
        }
        if (survivalTile2Panel.anchoredPosition.y < -1600)
        {
            survivalTile2Panel.anchoredPosition =  new Vector2(0, survivalTile2Panel.anchoredPosition.y + 3200);
        }
    }

    public void MoveBgDown(float distance)
    {
        survivalTile1Panel.anchoredPosition =  new Vector2(0, survivalTile1Panel.anchoredPosition.y + distance);
        survivalTile2Panel.anchoredPosition =  new Vector2(0, survivalTile1Panel.anchoredPosition.y + distance);
    }
}
