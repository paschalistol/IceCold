using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float max = 50;
    [SerializeField] private SurvivalModeHandler survivalModeHandler;
    [SerializeField] private SurvivalPool survivalPool;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Hole"))
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            Ball.instance.RotateBallInHole(collider.transform.position);
            AudioPlayer audioPlayer = survivalPool.GetFromPool(PoolObject.AUDIO_PLAYER).GetComponent<AudioPlayer>();
            Hole hole;
            if (ClassicGameManager.instance.GetGameMode() == GameMode.classic)
            {
                hole = collider.GetComponent<Hole>();
                hole.PlayWithAudioPlayer(audioPlayer);
                hole.BallInHole();
                Ball.instance.StartAnimation("BallInHole");
            }
            else if (ClassicGameManager.instance.GetGameMode() == GameMode.survival)
            {
                hole = collider.transform.parent.GetComponent<Hole>();   
                hole.PlayWithAudioPlayer(audioPlayer);
                hole.BallInHole();
                Ball.instance.StartAnimation("BallInHoleSurvival");
            }
            // audioPlayer.PlayClip();
        }

        if (collider.CompareTag("TimePowerUp"))
        {
            survivalModeHandler.AddTime();
            collider.GetComponent<SurvivalPowerUp>().BackToPool();
            GiveExtraPoints(collider.GetComponent<SurvivalPowerUp>().PowerUpPoints);
            survivalPool.GetFromPool(PoolObject.CLOCK_PICKUP_PLAYER);
        }

        if (collider.CompareTag("ExtraPointsPowerUp"))
        {
            collider.GetComponent<SurvivalPowerUp>().BackToPool();
            GiveExtraPoints(collider.GetComponent<SurvivalPowerUp>().PowerUpPoints);
            survivalPool.GetFromPool(PoolObject.EXTRA_POINTS_PLAYER);
        }
    }

    private void OnEnable()
    {
        rb.maxAngularVelocity = max;
        // Debug.Log(rb.maxAngularVelocity);
    }

    private void GiveExtraPoints(int points)
    {
        ClassicGameManager.instance.AddExtraSurvivalPoints(points);
    }
}
