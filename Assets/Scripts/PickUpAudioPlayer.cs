using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAudioPlayer : SurvivalPoolObjectBase
{
    [SerializeField] private AudioSource audioSource;


    private void OnEnable()
    {
        audioSource.Play();
        StartCoroutine(WaitAndDisable());
    }

    private IEnumerator WaitAndDisable()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        survivalPool.AddToPool(type, gameObject);
    }

}
