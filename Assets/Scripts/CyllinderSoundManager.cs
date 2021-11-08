using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyllinderSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip tick;
    AudioSource audioSource;
    [SerializeField] private GameObject leftPivot, rightPivot;
    private float previousLeft, previousRight, previousLevelHeight;
    [SerializeField] private float soundOnHeightDifference = 0.03f;
    private IronBar player;
    [SerializeField] private GameObject survivalPool;
    private void Start()
    {
        audioSource =  GetComponent<AudioSource>();
        player = GetComponent<IronBar>();
        player.resettingBar += PlayClickUp;
        UpdatePivotPosition();
        player.resettingPivots += UpdatePivotPosition;
        previousLevelHeight = survivalPool.transform.position.y;
        player.barInPlaceEvent += StopSound;

    }
    private void Update()
    {
        if (!audioSource.isPlaying && player.AllowControls)
        {

            if (Mathf.Abs(previousLeft - leftPivot.transform.position.y) > soundOnHeightDifference)
            {
                previousLeft = leftPivot.transform.position.y;
                PlaySound();
            }
            else if (Mathf.Abs(previousRight - rightPivot.transform.position.y) > soundOnHeightDifference)
            {
                previousRight = rightPivot.transform.position.y;
                PlaySound();
            }
            else if ((previousLevelHeight - survivalPool.transform.position.y) > soundOnHeightDifference)
            {
                previousLevelHeight = survivalPool.transform.position.y;
                PlaySound();
            }
        }

        if (player.AllowControls && !player.BarMovingByPlayer())
        {
            audioSource.Stop();
        }
    }

    private void PlaySound()
    {
        PlaySound(tick, false, 1.22f);
        audioSource.Play();
    }
    private void UpdatePivotPosition()
    {
        previousLeft = leftPivot.transform.position.y;
        previousRight = rightPivot.transform.position.y;
    }

    private void PlayClickDown()
    {
        PlaySound(tick, true, 1.7f);
        audioSource.Play();
    }
    private void PlayClickUp()
    {
        PlaySound(tick, true, 1.5f);
        audioSource.Play();
    }
    private void StopSound()
    {
        audioSource.Stop();
    }

    private void PlaySound(AudioClip clip, bool loop, float pitch)
    {
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.pitch = pitch;
    }
}
