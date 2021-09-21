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
    [SerializeField] private Joystick leftJoystick;
    [SerializeField] private Joystick rightJoystick;
    [SerializeField, Range(0, 1)] private float threshold = 0.2f;
    private bool allowControls = false;
    public delegate void ResettingBar();
    public ResettingBar resettingBar;
    public delegate void ResettingPivots();
    public ResettingPivots resettingPivots;
    public bool AllowControls
    {
        get { return allowControls; }
    }
    [SerializeField] private Ball ball;
    private Vector3 leftPivotStart, rightPivotStart, barStart;
    private bool barRotated, barLocated;
    Animation animat;
    private void Start()
    {
        leftPivotStart = leftPivot.transform.position;
        rightPivotStart = rightPivot.transform.position;
        barStart = transform.position;
        animat = GetComponent<Animation>();
        ClassicGameManager.instance.startRound += StartRound;
        ClassicGameManager.instance.endRound += EndRound;
    }
    void Update()
    {
        if (allowControls)
        {

        Controls();
        }
        if (barLocated && barRotated)
        {

            barRotated = false;
            barLocated = false;

            animat.Play("ReStartIron");
        }
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
            transform.position = Vector3.MoveTowards(transform.position, barStart, 0.01f);
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
        animat.Play("StartIron");
    }

    private void StartBall()
    {
        ball.StartAnimation("StartBall");
    }
    private void Controls()
    {
        if (leftJoystick.Vertical > 0 && (Input.GetKey(KeyCode.UpArrow) || rightJoystick.Vertical > threshold) && leftPivot.transform.position.y < topHeight && rightPivot.transform.position.y < topHeight)
        {
            transform.parent = null;
            leftPivot.transform.parent = transform;
            rightPivot.transform.parent = transform;
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        else if (Input.GetKey(KeyCode.S) && (Input.GetKey(KeyCode.DownArrow) || rightJoystick.Vertical < -threshold) && leftPivot.transform.position.y > bottomHeight && rightPivot.transform.position.y > bottomHeight)
        {
            transform.parent = null;
            leftPivot.transform.parent = transform;
            rightPivot.transform.parent = transform;

            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
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
