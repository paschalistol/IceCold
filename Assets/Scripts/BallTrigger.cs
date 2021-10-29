using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float max = 50;
    private void OnTriggerEnter(Collider collider)
    {
        if (ClassicGameManager.instance.GetGameMode() == GameMode.classic)
        {
            collider.GetComponent<Hole>().BallInHole();
        }
        else if (ClassicGameManager.instance.GetGameMode() == GameMode.survival)
        {
            
            collider.transform.parent.GetComponent<Hole>().BallInHole();
        }
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        // Ball.instance.transform.localPosition = collider.transform.position;
        Ball.instance.RotateBallInHole(collider.transform.position);
        if (ClassicGameManager.instance.GetGameMode()==GameMode.classic)
        {
            Ball.instance.StartAnimation("BallInHole");
        }
        else if (ClassicGameManager.instance.GetGameMode()==GameMode.survival)
        {
            Ball.instance.StartAnimation("BallInHoleSurvival");
        }

    }

    private void OnEnable()
    {
        rb.maxAngularVelocity = max;
    }

    private void Update()
    {
        Debug.Log(rb.maxAngularVelocity);
    }
}
