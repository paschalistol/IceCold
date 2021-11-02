using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HoleGeneratorSurvival : MonoBehaviour
{
    private int noOfColumns = 5;
    private float distance;
    private float startX = -2f, startY = -0.5f;
    private static int rowNumber, column;
    private SurvivalPool survivalPool;
    [SerializeField, Range(0,1)] private float chance = 0.7f;
    [SerializeField] private float xRange = 0.15f, yRange = 0.2f;
    private float startPoolY;
    private float previousClockAppearance = 0;
    [SerializeField, Range(0,1)] private float clockChance = 0.3f;
    private int rowChange = 0, minRow = 10;
    private GameObject objectHandler;
    private void Start()
    {
        rowNumber = 0;
        column = 0;
        startPoolY = transform.position.y;
        survivalPool = GetComponent<SurvivalPool>();
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
        PowerUpHandler();
        if (!activated)
        {
            NewHolePosition(holeTransform);
        }
    }

    private void PowerUpHandler()
    {
        if (rowChange<rowNumber && rowNumber > minRow)
        {
            TimePowerUpHandler();
            rowChange = rowNumber;
        }
    }

    private void TimePowerUpHandler()
    {
        if ( Random.Range(0f, 1)  < ((rowNumber-previousClockAppearance)/10 * clockChance))
        {
            // Debug.Log("<color=green>appeared in row: </color>" +rowNumber + " "+ c +" " + (rowNumber-previousClockAppearance));
            PutClock();
            previousClockAppearance = rowNumber;
        }
    }

    private void PutClock()
    {
        int randomColumn = Random.Range(0, noOfColumns-1);
        float randomX = startX + 0.5f + (randomColumn );
        float randomY = startY + (rowNumber * 1.4f) + transform.position.y - startPoolY;
        randomY += Random.Range(-yRange, yRange);
        Vector3 newPosition = new Vector3(randomX, randomY, 0);
        objectHandler = survivalPool.GetFromPool(PoolObject.CLOCK, newPosition);
        objectHandler.GetComponent<SurvivalPowerUp>().PowerUpInit(PoolObject.CLOCK, survivalPool);
    }
}
