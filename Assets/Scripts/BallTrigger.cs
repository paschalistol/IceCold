using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    private void OnTriggerEnter(Collider collider)
    {

        //transform.parent.gameObject.SetActive(false);
        collider.GetComponent<Hole>().BallInHole();
        rigidbody.velocity = Vector3.zero;
        rigidbody.useGravity = false;
        Ball.instance.transform.localPosition = collider.transform.position;
        Ball.instance.StartAnimation("BallInHole");

    }
}
