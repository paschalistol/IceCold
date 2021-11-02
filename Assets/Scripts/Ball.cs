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
    public delegate void StartingBall();
    public StartingBall startingBall;
    private Rigidbody rb;
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
    private void ActivateTrigger(bool activate)
    {
        rb.useGravity = activate;
        trigger.SetActive(activate);
    }

}
