using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronBar : MonoBehaviour
{

    [SerializeField]private float rotationSpeed = 1;
    [SerializeField] private float heightDifferenceOnSides = 0.5f;
    [SerializeField] private GameObject leftPivot, rightPivot;
    [SerializeField] private float bottomHeight = -2.5f;
    [SerializeField] private float topHeight = 3.2f;

    void Update()
    {
        Controls();
    }

    private void Controls()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.UpArrow) && leftPivot.transform.position.y < topHeight && rightPivot.transform.position.y < topHeight)
        {
            transform.parent = null;
            leftPivot.transform.parent = transform;
            rightPivot.transform.parent = transform;
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.DownArrow) && leftPivot.transform.position.y > bottomHeight && rightPivot.transform.position.y > bottomHeight)
        {
            transform.parent = null;
            leftPivot.transform.parent = transform;
            rightPivot.transform.parent = transform;

            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.DownArrow) && rightPivot.transform.position.y > bottomHeight)
        {
            if (leftPivot.transform.position.y - rightPivot.transform.position.y < heightDifferenceOnSides)
            {
                leftPivot.transform.parent = null;
                transform.parent = leftPivot.transform;
                rightPivot.transform.parent = transform;
                leftPivot.transform.rotation = Quaternion.Euler(0, 0, leftPivot.transform.eulerAngles.z + -rotationSpeed * Time.deltaTime);
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow) && rightPivot.transform.position.y < topHeight)
        {
            if (rightPivot.transform.position.y - leftPivot.transform.position.y < heightDifferenceOnSides)
            {
                leftPivot.transform.parent = null;
                transform.parent = leftPivot.transform;
                rightPivot.transform.parent = transform;
                leftPivot.transform.rotation = Quaternion.Euler(0, 0, leftPivot.transform.eulerAngles.z + rotationSpeed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.S) && leftPivot.transform.position.y > bottomHeight)
        {
            if (rightPivot.transform.position.y - leftPivot.transform.position.y < heightDifferenceOnSides)
            {
                rightPivot.transform.parent = null;
                transform.parent = rightPivot.transform;
                leftPivot.transform.parent = transform;
                rightPivot.transform.rotation = Quaternion.Euler(0, 0, rightPivot.transform.eulerAngles.z + rotationSpeed * Time.deltaTime);
            }
        }
        else if (Input.GetKey(KeyCode.W) && leftPivot.transform.position.y < topHeight)
        {
            if (leftPivot.transform.position.y - rightPivot.transform.position.y < heightDifferenceOnSides)
            {
                rightPivot.transform.parent = null;
                transform.parent = rightPivot.transform;
                leftPivot.transform.parent = transform;
                rightPivot.transform.rotation = Quaternion.Euler(0, 0, rightPivot.transform.eulerAngles.z + -rotationSpeed * Time.deltaTime);
            }
        }
    }
}
