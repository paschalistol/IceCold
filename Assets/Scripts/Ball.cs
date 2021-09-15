using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    float ballRandomPos = 2.18f;
    Animation anim;
    public static Ball instance;
    [SerializeField] private GameObject trigger;
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
    }
    private float offset = 0.005f;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
        }
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
    public void ActivateTrigger(bool activate)
    {
        trigger.SetActive(activate);
    }
}
