using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float offset = 0.005f;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);

        }
    }

}
