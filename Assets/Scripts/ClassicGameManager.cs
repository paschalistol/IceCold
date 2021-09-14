using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class ClassicGameManager : MonoBehaviour
{
    private int totalPoints = 0;
    private int currentGoal = 0;

    public static ClassicGameManager instance;
    private List<BonusHole> bonusHoles = new List<BonusHole>();
    
    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (instance != this)
            Destroy(gameObject);

    }
    public void AddBonus(BonusHole bonusHole)
    {
        bonusHoles.Add(bonusHole);
        if (bonusHoles.Count == 1)
        {
            bonusHoles[0].SetActiveGoal(true);
            currentGoal = 0;
        }
    }
    public void BallInBonus()
    {
        bonusHoles[currentGoal].SetActiveGoal(false);
        currentGoal++;
        Debug.Log("Goal");
        if (currentGoal <= bonusHoles.Count)
        {
            bonusHoles[currentGoal].SetActiveGoal(true) ;
        }
    }
}
