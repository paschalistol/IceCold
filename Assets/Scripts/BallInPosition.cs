using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInPosition : MonoBehaviour
{
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.y >-0.1f)
        {
            Debug.Log("Ball In Position");
            this.enabled = false;
        }
    }
}
