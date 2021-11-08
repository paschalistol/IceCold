using System;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Hole : MonoBehaviour
{
    [SerializeField]private AudioClip loseClip;
    private AudioSource audioSource;

    protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void BallInHole()
    {
        ClassicGameManager.instance.Die();
        PlaySound(loseClip);
    }

    protected void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
