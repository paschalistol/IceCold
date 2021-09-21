using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private void OnTriggerEnter(Collider collider)
    {

        collider.GetComponent<Hole>().BallInHole();
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        Ball.instance.transform.localPosition = collider.transform.position;
        Ball.instance.StartAnimation("BallInHole");

    }
}
