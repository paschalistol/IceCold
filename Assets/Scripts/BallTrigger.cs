using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {

            Debug.Log("Die");
            transform.parent.gameObject.SetActive(false);
        
    }
}
