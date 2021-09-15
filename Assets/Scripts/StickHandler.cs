using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickHandler : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private float startRotation = -45;

    private void Update()
    {
        transform.localEulerAngles = new Vector3(startRotation + joystick.Vertical *30 ,0,0);
    }
}
