using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalBGHandler : MonoBehaviour
{
    [SerializeField] private RectTransform classicModeBackground, survivalTile1Panel, survivalTile2Panel;
    private Color bgColor;
    [SerializeField] private SpriteRenderer survivalTile1, survivalTile2;
    private bool tile1IsUp;
    private float tile1Y;

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
            tile1IsUp = true;
        }
        if (survivalTile2Panel.anchoredPosition.y < -1600)
        {
            survivalTile2Panel.anchoredPosition =  new Vector2(0, survivalTile2Panel.anchoredPosition.y + 3200);
            tile1IsUp = false;
        }
    }

    public void MoveBgDown(float distance)
    {
        distance *= 180;
        tile1Y = survivalTile1Panel.anchoredPosition.y;
        survivalTile1Panel.anchoredPosition =  new Vector2(0, tile1Y + distance);
        survivalTile2Panel.anchoredPosition =  new Vector2(0, (tile1IsUp ? tile1Y +1600 + distance : tile1Y -1600 + distance));
    }
}
