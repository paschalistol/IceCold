using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class ClassicGameManager : MonoBehaviour
{
    private int totalPoints = 0;
    private int currentGoal = 0;
    private int currentPoints = 100;
    [SerializeField] private TMP_Text pointScreen;
    [SerializeField] private TMP_Text bonusPointScreen;
    [SerializeField] private TMP_Text livesScreen;
    public static ClassicGameManager instance;
    private List<BonusHole> bonusHoles = new List<BonusHole>();
    [SerializeField]private int lives = 0;
    [SerializeField] private int waitTime = 6;
    Coroutine pointDecreaser;
    private BoxCollider boxCollider;
    [SerializeField] private IronBar player;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (instance != this)
            Destroy(gameObject);
        boxCollider = GetComponent<BoxCollider>();
    }
    private void StartGame()
    {
        pointDecreaser = StartCoroutine(PointDecrease());
        boxCollider.enabled = false;
        Ball.instance.ActivateTrigger(true);
        player.StartRound();
        bonusPointScreen.SetText(currentPoints.ToString());
        livesScreen.SetText(lives.ToString());
    }
    private void StartRound()
    {
        boxCollider.enabled = true ;

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

        Debug.Log("die");
        lives--;

        if (lives < 0)
        {
            Debug.Log("Die");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            RestartBall();
        }
    }
    private void RestartBall()
    {
        StopCoroutine(pointDecreaser);
        player.EndRound();
        boxCollider.enabled = true;
        Ball.instance.ActivateTrigger(false);
    }
    public void BallInBonus()
    {
        RestartBall();
        bonusHoles[currentGoal].SetActiveGoal(false);
        UpdatePoints();
        currentGoal++;
        Debug.Log("Goal");
        if (currentGoal <= bonusHoles.Count)
        {
            bonusHoles[currentGoal].SetActiveGoal(true) ;
            currentPoints=(currentGoal + 1) * 100;
        }
    }

    private void UpdatePoints()
    {
        totalPoints += currentPoints;
        pointScreen.SetText(totalPoints.ToString());
    }
    IEnumerator PointDecrease()
    {
        while (currentPoints > 0)
        {
            yield return new WaitForSeconds(waitTime);
            currentPoints -= 10;
            Debug.Log(currentPoints);
            bonusPointScreen.SetText(currentPoints.ToString());
        }


    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            StartGame();
        }
    }
    
}
