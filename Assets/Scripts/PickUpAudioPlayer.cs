using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAudioPlayer : SurvivalPoolObjectBase
{
    [SerializeField] protected AudioSource audioSource;


    protected virtual void OnEnable()
    {
        audioSource.Play();
        StartCoroutine(WaitAndDisable());
    }

    protected IEnumerator WaitAndDisable()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        survivalPool.AddToPool(type, gameObject);
    }

}
