using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using System;
using EasyMobile;

public class ClassicGameManager : MonoBehaviour
{
    private int totalPoints = 0;
    private int currentGoal = 0;
    private int extraSurvivalPoints = 0;
    private int currentPoints = 100;
    [SerializeField] private TMP_Text pointScreen;
    [SerializeField] private TMP_Text bonusPointScreen;
    [SerializeField] private TMP_Text livesScreen;
    public static ClassicGameManager instance;
    private List<BonusHole> bonusHoles = new List<BonusHole>();
    [SerializeField]private int lives = 0;
    [SerializeField]private int survivalLives = 1;
    [SerializeField, Tooltip("Wait time for the bonus decrease")] private int waitTime = 6;
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
    public delegate void InitializingTexts();
    public InitializingTexts initializingTexts;
    public delegate void EndGame(bool outOfLives);
    public EndGame endGame;
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private int secondChanceLives = 2;
    [SerializeField] private int secondChanceLivesSurvival = 0;
    private bool gotAward;
    [SerializeField] private AdManager adManager;
    [SerializeField] private GameObject endPanel, endPanel2,optionsBG;
    [SerializeField] private TMP_Text highScore, survivalScore;
    private GameMode gameMode;
    [SerializeField] private GameObject survivalPool;
    [SerializeField] private TMP_Text debug;
    public float BallStartHeight
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (instance != this)
            Destroy(gameObject);
        boxCollider = GetComponent<BoxCollider>();
        Time.timeScale = 1;
    }
    public void BeginGame()
    {

        Time.timeScale = 1;
        masterMixer.SetFloat("sfxVolume", PlayerPrefs.GetFloat("sfxVolume",0));
    }
    public void BeginClassicGame()
    {
        bonusHoles[currentGoal].SetActiveGoal(true);
        gameMode = GameMode.classic;
        beginningGame();
    }
    public void BeginSurvivalGame()
    {
        gameMode = GameMode.survival;
        lives = survivalLives;
        beginningGame();
        secondChanceLives = secondChanceLivesSurvival;
        extraSurvivalPoints = 0;
    }
    private void Start()
    {
        adManager.giveReward += SecondChance;
        SetHighScores();
        player.startingBar += InitRoundTexts;
    }

    private void SetHighScores()
    {
        highScore.SetText(PlayerPrefs.GetInt("HighScore").ToString());
        survivalScore.SetText(PlayerPrefs.GetInt("Survival",0).ToString());
    }

    public void SetDebug(string text)
    {
        debug.text = text;
    }
    private void InitRoundTexts()
    {
        if (gameMode == GameMode.classic)
        {
            bonusPointScreen.SetText(currentPoints.ToString());
        }
        else if (gameMode == GameMode.survival)
        {
            initializingTexts();
        }
        livesScreen.SetText((lives -1).ToString());

    }
    private void StartGame()
    {
        pointDecreaser = StartCoroutine(PointDecrease());
        boxCollider.enabled = false;
        activateBallTrigger(true);
        startRound();

    }
    private void StartSurvivalGame()
    {
        activateBallTrigger(true);
        startRound();
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
        endGame(lives ==0);
        beginningGame();
        optionsBG.gameObject.SetActive(false);
    }
    public GameMode GetGameMode()
    {
        return gameMode;
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
            if (gameMode == GameMode.classic && totalPoints > (PlayerPrefs.GetInt("HighScore", 0)))
            {
                PlayerPrefs.SetInt("HighScore", totalPoints);
                highScore.SetText(PlayerPrefs.GetInt("HighScore", 0).ToString());
            }

            if (gameMode == GameMode.survival &&totalPoints > (PlayerPrefs.GetInt("Survival", 0)))
            {
                PlayerPrefs.SetInt("Survival", totalPoints);
                survivalScore.SetText(PlayerPrefs.GetInt("Survival", 0).ToString());
                GameServices.ReportScore(totalPoints, EM_GameServicesConstants.Leaderboard_Survival_High_Score);
            }
        }
    }

    public bool GetAllowControls()
    {
        return player.AllowControls;
    }
    public void NoReward()
    {
        gotAward = true;

        endPanel.SetActive(false);
        OutOfLives();
    }
    public void Die()
    {
        lives--;
        RestartBall();
        endGame( lives==0);
        if (lives == 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            OutOfLives();
        }

    }
    private void RestartBall()
    {
        if (gameMode == GameMode.classic)
        {
            StopCoroutine(pointDecreaser);
        }
        endRound();
        //boxCollider.enabled = true;
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

    private void Update()
    {
        if (gameMode == GameMode.survival)
        {
            UpdateSurvivalPoints();
        }
    }

    public void AddExtraSurvivalPoints(int points)
    {
        extraSurvivalPoints += points;
    }

    private void UpdateSurvivalPoints()
    {
        totalPoints = Mathf.Abs((int)(survivalPool.transform.position.y * 10) - 30);
        totalPoints += extraSurvivalPoints;
        pointScreen.SetText(totalPoints.ToString());
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
            //StartGame();
        }
    }

    public void BallInPosition()
    {
        BallStartHeight = Ball.instance.transform.position.y;
        if (gameMode == GameMode.classic)
        {
            StartGame();
        }
        else
        {
            StartSurvivalGame();
        }

    }
    
}
public enum GameMode{
    classic, survival
}
