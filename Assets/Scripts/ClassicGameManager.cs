using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using EasyMobile;
using UnityEngine.SocialPlatforms;

public class ClassicGameManager : MonoBehaviour
{
    private int totalPoints = 0;
    [SerializeField]private int currentGoal = 0;
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
    [SerializeField] private GameObject endPanel, endPanel2,optionsBG, askToSignInPanel;
    [SerializeField] private TMP_Text highScore, survivalScore;
    private GameMode gameMode;
    [SerializeField] private GameObject survivalPoolObject;
    [SerializeField] private GameObject winPopUp, winPopUpNotSignedIn;
    [SerializeField] private GameObject didntSignInClassic, didntSignInLose;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private TMP_Text statsDistance, statsTotal, statsCollected;
    private SurvivalPool survivalPool;
    [Header("SFX")] 
    [SerializeField] private AudioMixerGroup endRoundMixer;
    [SerializeField] private AudioClip endRoundClip;
    [SerializeField] private AudioMixerGroup outOfLivesMixer;
    [SerializeField] private AudioClip outOfLivesClip;
    [SerializeField] private AudioMixerGroup winClassicMixer;
    [SerializeField] private AudioClip winClassicClip;

    #region Achievements
    private List<Action> achievementsAfterInit = new List<Action>();
    [Header("Achievements")] 
    [SerializeField] private int timesToRevive = 5;
    [SerializeField] private int holeInClassicWithoutLosingLife = 5;
    private bool lostLife = false;
    [SerializeField] private int sprinterAchievementPoints = 5000;
    [SerializeField] private int powerUpsToCollect = 50;
    [SerializeField] private int aficionadoDistance = 1000;
    [SerializeField] private int veteranPoints = 4000;
    [SerializeField] private int marathonDistance = 5000;
    [SerializeField] private int survivorPoints = 20000;
    private int totalPowerUps = 0;
    private bool aficionadoChecked = false;
    #endregion
    
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
        GameServices.UserLoginSucceeded += SetHighScores;
        GameServices.UserLoginSucceeded += CheckCompletedAchievements;
        SetStartHighScores();
        adManager.giveReward += SecondChance;
        player.startingBar += InitRoundTexts;
        survivalPool = survivalPoolObject.GetComponent<SurvivalPool>();
    }
    private void OnDestroy()
    {
        GameServices.UserLoginSucceeded -= SetHighScores;
        GameServices.UserLoginSucceeded -= SaveNewHighScore;
        GameServices.UserLoginFailed -= LoginFailed;
        GameServices.UserLoginSucceeded -= CheckCompletedAchievements;
    }



    private void SetHighScores()
    {
        highScore.SetText(PlayerPrefs.GetInt("HighScore").ToString());
        survivalScore.SetText(PlayerPrefs.GetInt("Survival",0).ToString());
        GameServices.LoadLocalUserScore(EM_GameServicesConstants.Leaderboard_Classic_Personal_Best, OnLocalUserScoreLoaded);
        GameServices.LoadLocalUserScore(EM_GameServicesConstants.Leaderboard_Survival_High_Score, OnLocalUserScoreLoaded);
    }
    private void SetStartHighScores()
    {
        highScore.SetText(PlayerPrefs.GetInt("HighScore").ToString());
        survivalScore.SetText(PlayerPrefs.GetInt("Survival",0).ToString());
    }
    void OnLocalUserScoreLoaded(string leaderboardName, IScore score) {
        switch (leaderboardName)
        {
            case EM_GameServicesConstants.Leaderboard_Survival_High_Score:
                SetHighScore(survivalScore, score, "Survival");
                break;
            case EM_GameServicesConstants.Leaderboard_Classic_Personal_Best:
                SetHighScore(highScore, score, "HighScore");
                break;
        }
    }

    private void SetHighScore(TMP_Text tmpText, IScore score, string playerPref)
    {
        if (score != null && score.value > PlayerPrefs.GetInt(playerPref)) {
            tmpText.SetText(score.formattedValue);
            PlayerPrefs.SetInt(playerPref, (int)score.value);
        } else {
            tmpText.SetText(PlayerPrefs.GetInt(playerPref,0).ToString());
        }
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
        if (GameServices.IsInitialized())
        {
            RevivalAchievement();
        }
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
            if (gameMode == GameMode.classic && totalPoints > (PlayerPrefs.GetInt("HighScore", 0)))
            {
                PlayerPrefs.SetInt("HighScore", totalPoints);
                highScore.SetText(PlayerPrefs.GetInt("HighScore", 0).ToString());
            }

            if (gameMode == GameMode.survival &&totalPoints > (PlayerPrefs.GetInt("Survival", 0)))
            {
                PlayerPrefs.SetInt("Survival", totalPoints);
                survivalScore.SetText(PlayerPrefs.GetInt("Survival", 0).ToString());
                CompleteCollector();
                CompleteMarathon();
                CompleteSurvivor();
                CompleteVeteran();
            }
            if (GameServices.IsInitialized())
            {
                
                SaveNewHighScore();
                endPanel2.SetActive(true);
            }
            else
            {
                askToSignInPanel.SetActive(true);
            }

            if (gameMode == GameMode.survival)
            {
                statsPanel.SetActive(true);
                statsCollected.text = totalPowerUps.ToString();
                statsTotal.text = totalPoints.ToString();
                statsDistance.text = (totalPoints - extraSurvivalPoints).ToString();
            }
        }
    }

    private void WinClassic()
    {
        CompleteClassicAchievement();
        CompleteClassicImmortalAchievement();
        CompleteClassicSprinterAchievement();
        if (totalPoints > (PlayerPrefs.GetInt("HighScore", 0)))
        {
            PlayerPrefs.SetInt("HighScore", totalPoints);
            highScore.SetText(PlayerPrefs.GetInt("HighScore", 0).ToString());
        }
        if (GameServices.IsInitialized())
        {
            SaveNewHighScore();
            winPopUp.SetActive(true);
        }
        else
        {
            winPopUpNotSignedIn.SetActive(true);
        }
    }

    public void AskToSignInBeforeHome()
    {
        GameServices.UserLoginSucceeded += SaveNewHighScore;
        GameServices.UserLoginFailed += LoginFailed;
        GameServices.Init();
    }

    private void CheckCompletedAchievements()
    {
        foreach (var action in achievementsAfterInit)
        {
            action();
        }
    }

    private void LoginFailed()
    {
        if (gameMode == GameMode.classic)
        {
            winPopUpNotSignedIn.SetActive(false);
            winPopUp.SetActive(true);
            didntSignInClassic.SetActive(true);
        }
        else if (gameMode == GameMode.survival)
        {
            askToSignInPanel.SetActive(false);
            endPanel2.SetActive(true);
            didntSignInLose.SetActive(true);
        }

    }

    private void SaveNewHighScore()
    {
        if (gameMode == GameMode.classic)
        {
            GameServices.ReportScore(totalPoints, EM_GameServicesConstants.Leaderboard_Classic_Personal_Best);
            if (winPopUpNotSignedIn.gameObject.activeSelf)
            {
                winPopUpNotSignedIn.SetActive(false);
                winPopUp.SetActive(true);
            }
            else
            {
                askToSignInPanel.SetActive(false);
                endPanel2.SetActive(true);
            }

        }

        else if (gameMode == GameMode.survival)
        {
            GameServices.ReportScore(totalPoints, EM_GameServicesConstants.Leaderboard_Survival_High_Score);
            askToSignInPanel.SetActive(false);
            endPanel2.SetActive(true);
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
        lostLife = true;
        lives--;
        RestartBall();
        endGame( lives==0);
        if (lives == 0)
        {
            OutOfLives();
            survivalPool.GetFromPool(PoolObject.AUDIO_PLAYER).GetComponent<AudioPlayer>().PlayClip(outOfLivesClip, outOfLivesMixer);
        }
        else
        {
            survivalPool.GetFromPool(PoolObject.AUDIO_PLAYER).GetComponent<AudioPlayer>().PlayClip(endRoundClip, endRoundMixer);
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
        if (currentGoal == holeInClassicWithoutLosingLife && !lostLife)
        {
            GhostAchievement();
        }
        if (currentGoal < bonusHoles.Count)
        {
            bonusHoles[currentGoal].SetActiveGoal(true) ;
            currentPoints=(currentGoal + 1) * 100;
        }
        else if (currentGoal == bonusHoles.Count)
        {
            endGame(true);
            survivalPool.GetFromPool(PoolObject.AUDIO_PLAYER).GetComponent<AudioPlayer>().PlayClip(winClassicClip, winClassicMixer);
            WinClassic();
            //unlock achievement
            //to remove [serialize field] from variable: currentgoal
        }
    }
    
    private void Update()
    {
        if (gameMode == GameMode.survival)
        {
            UpdateSurvivalPoints();
            if (!aficionadoChecked && !lostLife && (totalPoints - extraSurvivalPoints) >= aficionadoDistance )
            {
                aficionadoChecked = true;
                CompleteAficionado();
            }
        }
    }

    public void AddExtraSurvivalPoints(int points)
    {
        extraSurvivalPoints += points;
        totalPowerUps++;
    }

    private void UpdateSurvivalPoints()
    {
        totalPoints = Mathf.Abs((int)(survivalPoolObject.transform.position.y * 10) - 30);
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

    #region Achievement Code
    private void RevivalAchievement()
    {
        if (GameServices.IsInitialized())
        {
            int timesRevived = PlayerPrefs.GetInt("RevivalTimes", 0);
            timesRevived++;
            PlayerPrefs.SetInt("RevivalTimes", timesRevived);
            if (timesRevived >= timesToRevive)
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Resurrector);
            }
        }
        else
        {
            achievementsAfterInit.Add(RevivalAchievement);
        }

    }
    /// <summary>
    /// Get to <paramref name="holeInClassicWithoutLosingLife"/> without losing a life to unlock this achievement
    /// </summary>
    private void GhostAchievement()
    {
        if (GameServices.IsInitialized())
        {
            GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Ghost);
        }
        else
        {
           achievementsAfterInit.Add(GhostAchievement);
        }
    }

    private void CompleteClassicAchievement()
    {
        if (GameServices.IsInitialized())
        {
            GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Finisher);
        }
        else
        {
            achievementsAfterInit.Add(CompleteClassicAchievement);
        }
    }

    private void CompleteClassicImmortalAchievement()
    {
        if (GameServices.IsInitialized())
        {
            if (!lostLife)
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Immortal);
            }
        }
        else
        {
            achievementsAfterInit.Add(CompleteClassicImmortalAchievement);
        }
    }
    private void CompleteClassicSprinterAchievement()
    {
        if (GameServices.IsInitialized())
        {
            if (totalPoints >= sprinterAchievementPoints) 
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Sprinter);
            }
        }
        else
        {
            achievementsAfterInit.Add(CompleteClassicSprinterAchievement);
        }
    }

    private void CompleteAficionado()
    {
        if (GameServices.IsInitialized())
        {
            GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Aficionado);
        }
        else
        {
            achievementsAfterInit.Add(CompleteAficionado);
        }
    }
    public void CompleteTimeKeeper()
    {
        if (GameServices.IsInitialized())
        {
            GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Time_Keeper);
        }
        else
        {
            achievementsAfterInit.Add(CompleteTimeKeeper);
        }
    }

    private void CompleteVeteran()
    {
        if (GameServices.IsInitialized())
        {
            if (totalPoints >= veteranPoints) 
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Veteran);
            }
        }
        else
        {
            achievementsAfterInit.Add(CompleteVeteran);
        }
    }

    private void CompleteCollector()
    {
        if (GameServices.IsInitialized())
        {
            if (totalPowerUps >= powerUpsToCollect) 
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Collector);
            }
        }
        else
        {
            achievementsAfterInit.Add(CompleteCollector);
        }
    }

    private void CompleteMarathon()
    {
        if (GameServices.IsInitialized())
        {
            if ((totalPoints - extraSurvivalPoints) >= marathonDistance) 
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Marathon_Runner);
            }
        }
        else
        {
            achievementsAfterInit.Add(CompleteMarathon);
        }
    }

    private void CompleteSurvivor()
    {
        if (GameServices.IsInitialized())
        {
            if (totalPoints >= survivorPoints) 
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Survivor);
            }
        }
        else
        {
            achievementsAfterInit.Add(CompleteSurvivor);
        }
    }
    #endregion
    
}
public enum GameMode{
    classic, survival
}
