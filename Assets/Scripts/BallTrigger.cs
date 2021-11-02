using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float max = 50;
    [SerializeField] private SurvivalModeHandler survivalModeHandler;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Hole"))
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            Ball.instance.RotateBallInHole(collider.transform.position);
            if (ClassicGameManager.instance.GetGameMode() == GameMode.classic)
            {
                collider.GetComponent<Hole>().BallInHole();
                Ball.instance.StartAnimation("BallInHole");
            }
            else if (ClassicGameManager.instance.GetGameMode() == GameMode.survival)
            {
            
                collider.transform.parent.GetComponent<Hole>().BallInHole();
                Ball.instance.StartAnimation("BallInHoleSurvival");
            }
        }

        if (collider.CompareTag("TimePowerUp"))
        {
            survivalModeHandler.AddTime();
            collider.GetComponent<SurvivalPowerUp>().BackToPool();
        }

    }

    private void OnEnable()
    {
        rb.maxAngularVelocity = max;
    }

    private void Update()
    {
        // Debug.Log(rb.maxAngularVelocity);
    }
}
