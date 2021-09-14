using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {

        //transform.parent.gameObject.SetActive(false);
        collider.GetComponent<Hole>().BallInHole();
    }
}
