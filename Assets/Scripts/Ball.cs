using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Ball : MonoBehaviour
{
    float ballRandomPos = 2.18f;
    Animation anim;
    public static Ball instance;
    [SerializeField] private GameObject trigger;
    private AudioSource audioSource;
    public delegate void StartingBall();
    public StartingBall startingBall;
    private Rigidbody rb;
    private float startingHeight = 4;

    [Header("Ball Landing")] 
    [SerializeField] private AudioClip ballLanding;
    [SerializeField] private AudioMixerGroup ballLandingMixer;
    [Header("Ball Going Up")] 
    [SerializeField] private AudioClip ballGoingUp;
    [SerializeField] private AudioMixerGroup ballGoingUpMixer;
    [SerializeField] private AudioMixerSnapshot fadeOutSnapshot;
    [SerializeField] private AudioMixerSnapshot startSnapshot;
    
    public bool BallResetting
    {
        get;
        private set;
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (instance != this)
            Destroy(gameObject);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animation>();
        ClassicGameManager.instance.activateBallTrigger += ActivateTrigger;
        audioSource = GetComponent<AudioSource>();
        ClassicGameManager.instance.startRound += PlayBallSoundLanding;

    }
    private float offset = 0.002f;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
        }
    }
    private void PlayBallSoundLanding()
    {
        audioSource.clip = ballLanding;
        audioSource.outputAudioMixerGroup = ballLandingMixer;
        audioSource.Play();
    }
    public void StartAnimation(string animation)
    {

        if (animation.Equals("StartBall"))
        {
            float randomX = Random.Range(-ballRandomPos, ballRandomPos);
            transform.position = new Vector3( randomX,startingHeight, 0);
            if (ClassicGameManager.instance.GetGameMode() == GameMode.survival)
            {
                transform.localScale = Vector3.one * 2.5f;
            }else if (ClassicGameManager.instance.GetGameMode() == GameMode.classic)
            {
                transform.localScale = Vector3.one;
            }


        }
        anim.Play(animation);
    }

    public void BarInLocation()
    {
        if (startingBall != null)
        {
            startingBall();
        }
    }
    public void RotateBallInHole(Vector3 holeCenter)
    {
        StartCoroutine(BallInHoleAnimator(holeCenter));
    }
    IEnumerator BallInHoleAnimator(Vector3 holeCenter)
    {
        BallResetting = true;
        while (transform.localScale.x>0)
        {
            transform.RotateAround(holeCenter, -Vector3.forward, 300 * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, holeCenter, 0.1f * Time.deltaTime);
            yield return null;
        }
        BallResetting = false;
    }

    public void TimeEnded(float speed)
    {
        PlayBallGoingUpSound();
        StartCoroutine(TimeEndedEnumarator( speed));
    }

    private void PlayBallGoingUpSound()
    {
        startSnapshot.TransitionTo(0);
        audioSource.clip = ballGoingUp;
        audioSource.outputAudioMixerGroup = ballGoingUpMixer;
        audioSource.Play();
    }

    IEnumerator TimeEndedEnumarator(float speed)
    {
        BallResetting = true;
        rb.velocity = Vector3.zero;
        while (transform.position.y < startingHeight)
        {
            // transform.Translate(Vector3.up * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, startingHeight), speed * Time.deltaTime);
            yield return null;
        }
        fadeOutSnapshot.TransitionTo(0.5f);
        BallResetting = false;
    }
    private void ActivateTrigger(bool activate)
    {
        rb.useGravity = activate;
        trigger.SetActive(activate);
    }

}
