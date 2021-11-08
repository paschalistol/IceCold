using System;
using UnityEngine;
using UnityEngine.Audio;

public class Hole : MonoBehaviour
{
    [SerializeField] private AudioClip loseClip;
    [SerializeField] private AudioMixerGroup mixerGroup;

    public virtual void PlayWithAudioPlayer(AudioPlayer audioPlayer)
    {
        audioPlayer.PlayClip(loseClip, mixerGroup);
    }

    public virtual void BallInHole()
    {
        ClassicGameManager.instance.Die();
    }

}
