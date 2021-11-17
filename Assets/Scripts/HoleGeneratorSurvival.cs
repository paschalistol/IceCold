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
    private float previousCoinAppearance = 0;
    [SerializeField, Range(0,1)] private float clockChance = 0.3f;
    [SerializeField, Range(0,1)] private float coinChance = 0.2f;
    private int rowChange = 0, minRow = 10;
    private int previousPowerUpAppearance = 0;
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

    private Vector3 GetRandomPosition()
    {
        int randomColumn = Random.Range(0, noOfColumns-1);
        float randomX = startX + 0.5f + (randomColumn );
        float randomY = startY + (rowNumber * 1.4f) + transform.position.y - startPoolY;
        randomY += Random.Range(-yRange, yRange);
        return new Vector3(randomX, randomY, 0);
    }
    private void PowerUpHandler()
    {
        if (rowChange<rowNumber && rowNumber > minRow)
        {
            TimePowerUpHandler();
            if (previousPowerUpAppearance != rowNumber)
            {
                ExtraPointsPowerUpHandler();
            }
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
            previousPowerUpAppearance = rowNumber;
            // CountPowerUps();
        }
    }

    private void PutClock()
    {
        survivalPool.GetFromPool(PoolObject.CLOCK, GetRandomPosition());
    }

    private void ExtraPointsPowerUpHandler()
    {
        if (Random.Range(0f, 1) < ((rowNumber - previousCoinAppearance) / 10 * coinChance))
        {
            PutExtraPointsCoin();
            previousCoinAppearance = rowNumber;
            previousPowerUpAppearance = rowNumber;
            // CountPowerUps();
        }
    }
    // private int totalPowerUps = 0;
    // private void CountPowerUps()
    // {
    //     totalPowerUps++;
    //     Debug.Log(totalPowerUps);
    // }

    // private int totalFromCoins = 0;
    private void PutExtraPointsCoin()
    {
        survivalPool.GetFromPool(PoolObject.EXTRA_POINTS, GetRandomPosition()).GetComponent<ExtraPointsPowerUp>().SetExtraPoints((int)Mathf.Ceil(Math.Abs(transform.position.y/2)));
        // totalFromCoins += (int) Mathf.Ceil(Math.Abs(transform.position.y / 2));
        // Debug.Log(totalFromCoins);
    }
}
