using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    float ballRandomPos = 2.18f;
    Animation anim;
    public static Ball instance;
    [SerializeField] private GameObject trigger;
    private AudioSource audioSource;
    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (instance != this)
            Destroy(gameObject);
    }
    private void Start()
    {
        anim = GetComponent<Animation>();
        ClassicGameManager.instance.activateBallTrigger += ActivateTrigger;
        audioSource = GetComponent<AudioSource>();
        ClassicGameManager.instance.startRound += PlayBallSound;
    }
    private float offset = 0.002f;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
        }
    }
    private void PlayBallSound()
    {
        audioSource.Play();
    }
    public void StartAnimation(string animation)
    {

        if (animation.Equals("StartBall"))
        {
            float randomX = Random.Range(-ballRandomPos, ballRandomPos);
            transform.position = new Vector3( randomX,4, 0);
            transform.localScale = Vector3.one;
        }
        anim.Play(animation);
    }
    private void ActivateTrigger(bool activate)
    {
        trigger.SetActive(activate);
    }

}
