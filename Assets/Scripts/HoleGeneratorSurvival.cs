using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HoleGeneratorSurvival : MonoBehaviour
{
    private int noOfColumns = 5;
    private float distance;
    private float startX = -2f, startY = -0.5f;
    private static int rowNumber, column;
    [SerializeField, Range(0,1)] private float chance = 0.7f;
    [SerializeField] private float xRange = 0.15f, yRange = 0.2f;
    private float startPoolY;

    private void Start()
    {
        startPoolY = transform.position.y;
    }

    public void NewHolePosition(Transform holeTransform)
    {
        bool activated = false ;
        rowNumber = column / noOfColumns;
        if (Random.Range(0f,1) < chance)
        {
            activated = true;
            float randomX = startX + (column % noOfColumns );
            randomX += Random.Range(-xRange, xRange);
            float randomY = startY + (rowNumber * 1.4f) + transform.position.y - startPoolY;
            randomY += Random.Range(-yRange, yRange);
            // Debug.Log(randomX + " " + randomY);
            holeTransform.position = new Vector3(randomX, randomY, 0);
        }
        column++;
        if (!activated)
        {
            NewHolePosition(holeTransform);
        }
    }
}
