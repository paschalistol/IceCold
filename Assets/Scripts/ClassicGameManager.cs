using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

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
    public delegate void StartRound();
    public StartRound startRound;
    public delegate void EndRound();
    public EndRound endRound;
    public delegate void ActivateBallTrigger(bool activate);
    public ActivateBallTrigger activateBallTrigger;
    public delegate void BeginningGame();
    public BeginningGame beginningGame;
    public delegate void EndGame(bool outOfLives);
    public EndGame endGame;
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private int secondChanceLives = 2;
    private bool gotAward;
    [SerializeField] private AdManager adManager;
    [SerializeField] private GameObject endPanel, endPanel2,optionsBG;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (instance != this)
            Destroy(gameObject);
        boxCollider = GetComponent<BoxCollider>();
    }
    public void BeginGame()
    {
        beginningGame();
        Time.timeScale = 1;
        masterMixer.SetFloat("sfxVolume", PlayerPrefs.GetFloat("sfxVolume",0));
        bonusHoles[currentGoal].SetActiveGoal(true);
    }
    private void Start()
    {
        adManager.giveReward += SecondChance;
    }
    private void StartGame()
    {
        pointDecreaser = StartCoroutine(PointDecrease());
        boxCollider.enabled = false;
        activateBallTrigger(true);
        startRound();
        bonusPointScreen.SetText(currentPoints.ToString());
        livesScreen.SetText(lives.ToString());
    }

    public void AddBonus(BonusHole bonusHole)
    {
        bonusHoles.Add(bonusHole);
        if (bonusHoles.Count == 1)
        {

            currentGoal = 0;
        }
    }
    private void SecondChance()
    {

            gotAward = true;
            endPanel.SetActive(false);
            lives = 0;
            lives += secondChanceLives;
            endGame(lives < 0);
            beginningGame();
            optionsBG.gameObject.SetActive(false);
    }
    private void OutOfLives()
    {
        optionsBG.gameObject.SetActive(true);
        if (!gotAward)
        {

            endPanel.SetActive(true);
        }
        else
        {
            endPanel2.SetActive(true);

        }
    }
    public void Die()
    {

        lives--;

        RestartBall();
        endGame( lives<0);
        if (lives < 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            OutOfLives();
        }

    }
    private void RestartBall()
    {
        StopCoroutine(pointDecreaser);
        endRound();
        boxCollider.enabled = true;
        activateBallTrigger(false);
        //Ball.instance.ActivateTrigger(false);
    }
    public void BallInBonus()
    {
        RestartBall();
        bonusHoles[currentGoal].SetActiveGoal(false);
        UpdatePoints();
        currentGoal++;
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
            bonusPointScreen.SetText(currentPoints.ToString());
        }


    }

    //Trigger when balls enter to enable controls and start point decreaser
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            StartGame();
        }
    }
    
}
