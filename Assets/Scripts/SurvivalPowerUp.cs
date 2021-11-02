using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalPowerUp : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private GameObject face;

    void Update()
    {
        face.transform.eulerAngles = new Vector3(0, face.transform.eulerAngles.y + Time.deltaTime * rotationSpeed, 0);
    }
}
