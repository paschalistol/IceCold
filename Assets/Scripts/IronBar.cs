using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronBar : MonoBehaviour
{

    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float heightDifferenceOnSides = 0.5f;
    [SerializeField] private GameObject leftPivot, rightPivot;
    [SerializeField] private float bottomHeight = -2.5f;
    [SerializeField] private float topHeight = 3.2f;
    [SerializeField] private float topHeightSurvival = -0.5f;
    [SerializeField] private Joystick leftJoystick;
    [SerializeField] private Joystick rightJoystick;
    [SerializeField, Range(0, 1)] private float threshold = 0.2f;
    [SerializeField] private SurvivalPool survivalPool;
    private bool allowControls = false;
    public delegate void ResettingBar();
    public ResettingBar resettingBar;
    public delegate void ResettingPivots();
    public ResettingPivots resettingPivots;
    private GameMode gameMode;
    [SerializeField] private SurvivalBGHandler survivalBgHandler;

    public bool AllowControls
    {
        get { return allowControls; }
    }
    [SerializeField] private Ball ball;
    private Vector3 leftPivotStart, rightPivotStart;
    [SerializeField] private Vector3 barStart;
    private bool barRotated, barLocated;
    Animation animat;
    private bool outOfLives = false;

    private void Start()
    {
        leftPivotStart = leftPivot.transform.position;
        rightPivotStart = rightPivot.transform.position;
        animat = GetComponent<Animation>();
        ClassicGameManager.instance.beginningGame += StartGame;
        ClassicGameManager.instance.startRound += StartRound;
        ClassicGameManager.instance.endRound += EndRound;
        ClassicGameManager.instance.endGame += OutOfLives;
    }

    private void OutOfLives(bool noMoreLives)
    {
        outOfLives = noMoreLives;
    }
    private void InitializeInvisible()
    {
        leftPivotStart = new Vector3(leftPivot.transform.position.x, transform.position.y, leftPivot.transform.position.z);
        rightPivotStart = new Vector3(rightPivot.transform.position.x, transform.position.y, rightPivot.transform.position.z);
        leftPivot.transform.position = leftPivotStart;
        rightPivot.transform.position = rightPivotStart;
    }
    private void StartGame()
    {
        gameMode = ClassicGameManager.instance.GetGameMode();
        if (gameMode == GameMode.survival)
        {
            topHeight = topHeightSurvival + heightDifferenceOnSides;
        }
        if (!animat.isPlaying)
        {
            StartNextRound();
        }
    }
    void FixedUpdate()
    {
        if (allowControls)
        {
            Controls();
            if (gameMode == GameMode.survival && transform.position.y > topHeightSurvival)
            {
                SurvivalControl();
                survivalPool.transform.position = new Vector3(survivalPool.transform.position.x,
                    survivalPool.transform.position.y - 0.01f, survivalPool.transform.position.z);
                survivalBgHandler.MoveBgDown(-0.01f);
            }
        }
        if (barLocated && barRotated && !Ball.instance.BallResetting)
        {

            barRotated = false;
            barLocated = false;
            animat.Play("ReStartIron");
        }


    }



    private void SurvivalControl()
    {
        transform.parent = null;
        leftPivot.transform.parent = transform;
        rightPivot.transform.parent = transform;
        transform.position = new Vector3(0, topHeightSurvival, transform.position.z);
    }

    private void EndRound()
    {
        allowControls = false;
        ResetBar();
    }

    private void StartRound()
    {
        allowControls = true;
    }
    private void ResetBar()
    {
        transform.parent = null;
        resettingBar();
        StartCoroutine(ResetBarRotation());
        StartCoroutine(ResetBarLocation());
    }
    
    IEnumerator ResetBarRotation()
    {
        while (transform.eulerAngles.z != 90)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,0,90), 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        barRotated = true;

    }
    IEnumerator ResetBarLocation()
    {
        while (transform.position.y != barStart.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, barStart, 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
        RestartPivots();
        barLocated = true;

    }
    private void RestartPivots()
    {
        leftPivot.transform.position = leftPivotStart;
        rightPivot.transform.position = rightPivotStart;
        leftPivot.transform.parent = null;
        rightPivot.transform.parent = null;
        resettingPivots();
    }
    private void StartNextRound()
    {
        if (!outOfLives)
        {
            if (gameMode == GameMode.classic)
            {
                animat.Play("StartIron"); 
            }
            else if (gameMode == GameMode.survival)
            {   
                ball.BarInLocation();
                animat.Play("StartIron"); 
            }
        }
    }

    private void StartBall()
    {
        ball.StartAnimation("StartBall");
    }
    private void Controls()
    {
        if ((leftJoystick.Vertical > threshold || Input.GetKey(KeyCode.W)) && (Input.GetKey(KeyCode.UpArrow) || rightJoystick.Vertical > threshold) && leftPivot.transform.position.y < topHeight && rightPivot.transform.position.y < topHeight)
        {
            transform.parent = null;
            leftPivot.transform.parent = transform;
            rightPivot.transform.parent = transform;
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        else if (leftJoystick.Vertical < -threshold || (Input.GetKey(KeyCode.S)) && (Input.GetKey(KeyCode.DownArrow) || rightJoystick.Vertical < -threshold) && leftPivot.transform.position.y > bottomHeight && rightPivot.transform.position.y > bottomHeight)
        {
            transform.parent = null;
            leftPivot.transform.parent = transform;
            rightPivot.transform.parent = transform;

            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        //only right
        if ((Input.GetKey(KeyCode.DownArrow) || rightJoystick.Vertical < -threshold) && rightPivot.transform.position.y > bottomHeight)
        {
            if (leftPivot.transform.position.y - rightPivot.transform.position.y < heightDifferenceOnSides)
            {
                leftPivot.transform.parent = null;
                transform.parent = leftPivot.transform;
                rightPivot.transform.parent = transform;
                leftPivot.transform.rotation = Quaternion.Euler(0, 0, leftPivot.transform.eulerAngles.z + -rotationSpeed * (Mathf.Abs(rightJoystick.Vertical) > 0 ? Mathf.Abs(rightJoystick.Vertical) : 1) * Time.deltaTime);
            }
        }
        else if ((Input.GetKey(KeyCode.UpArrow) || rightJoystick.Vertical > threshold) && rightPivot.transform.position.y < topHeight)
        {
            if (rightPivot.transform.position.y - leftPivot.transform.position.y < heightDifferenceOnSides)
            {
                leftPivot.transform.parent = null;
                transform.parent = leftPivot.transform;
                rightPivot.transform.parent = transform;
                leftPivot.transform.rotation = Quaternion.Euler(0, 0, leftPivot.transform.eulerAngles.z + rotationSpeed * (Mathf.Abs(rightJoystick.Vertical) > 0 ? Mathf.Abs(rightJoystick.Vertical) : 1) * Time.deltaTime);
            }
        }
        //only left
        if ((Input.GetKey(KeyCode.S) || leftJoystick.Vertical < -threshold) && leftPivot.transform.position.y > bottomHeight)
        {
            if (rightPivot.transform.position.y - leftPivot.transform.position.y < heightDifferenceOnSides)
            {
                rightPivot.transform.parent = null;
                transform.parent = rightPivot.transform;
                leftPivot.transform.parent = transform;
                rightPivot.transform.rotation = Quaternion.Euler(0, 0, rightPivot.transform.eulerAngles.z + rotationSpeed * (Mathf.Abs(leftJoystick.Vertical) > 0 ? Mathf.Abs(leftJoystick.Vertical) : 1) * Time.deltaTime);
            }
        }
        else if ((Input.GetKey(KeyCode.W) || leftJoystick.Vertical > threshold) && leftPivot.transform.position.y < topHeight)
        {
            if (leftPivot.transform.position.y - rightPivot.transform.position.y < heightDifferenceOnSides)
            {
                rightPivot.transform.parent = null;
                transform.parent = rightPivot.transform;
                leftPivot.transform.parent = transform;
                rightPivot.transform.rotation = Quaternion.Euler(0, 0, rightPivot.transform.eulerAngles.z + -rotationSpeed * (Mathf.Abs(leftJoystick.Vertical)>0 ? Mathf.Abs(leftJoystick.Vertical) : 1) * Time.deltaTime);;
            }
        }
    }
}
