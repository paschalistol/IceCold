using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;
public class ExtraPointsPowerUp : SurvivalPowerUp
{
    [SerializeField] [NotNull] private TMP_Text coinText1, coinText2;
    
    public void SetExtraPoints(int extraPoints)
    {
        powerUpPoints = extraPoints;
        coinText1.text = extraPoints.ToString();
        coinText2.text = extraPoints.ToString();
    }
}
