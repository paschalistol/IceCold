using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ClassicGameManager : MonoBehaviour
{
    private int totalPoints = 0;
    private int currentGoal = 0;
    private int currentPoints = 100;
    [SerializeField] private TMP_Text pointScreen;
    public static ClassicGameManager instance;
    private List<BonusHole> bonusHoles = new List<BonusHole>();
    [SerializeField]private int lives = 0;
    [SerializeField] private int waitTime = 6;
    Coroutine pointDecreaser;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (instance != this)
            Destroy(gameObject);
        StartGame();
    }
    private void StartGame()
    {
        pointDecreaser = StartCoroutine(PointDecrease());
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
    public void Die()
    {
        lives--;
        if (lives < 0)
        {
            Debug.Log("Die");
        }
    }
    public void BallInBonus()
    {
        StopCoroutine(pointDecreaser);
        bonusHoles[currentGoal].SetActiveGoal(false);
        UpdatePoints();
        currentGoal++;
        Debug.Log("Goal");
        if (currentGoal <= bonusHoles.Count)
        {
            bonusHoles[currentGoal].SetActiveGoal(true) ;
            currentPoints=(currentGoal + 1) * 100;
            pointDecreaser = StartCoroutine(PointDecrease());
        }
    }

    private void UpdatePoints()
    {
        totalPoints += currentPoints;
        pointScreen.SetText(totalPoints.ToString());
    }
    IEnumerator PointDecrease()
    {
        yield return new WaitForSeconds(waitTime);
        currentPoints -= 10;
        Debug.Log(currentPoints);
        if (currentPoints>0)
        {

            pointDecreaser = StartCoroutine(PointDecrease());
        }
    }
}
