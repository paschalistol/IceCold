using UnityEngine;

public class HoleGeneratorSurvival : MonoBehaviour
{
    private int noOfColumns = 5;
    private float distance;
    private float startX = -2f, startY = -0.5f;
    private int holeId;
    private static int rowNumber, column;
    [SerializeField, Range(0,1)] private float chance = 0.7f;
    [SerializeField] private float xRange = 0.15f, yRange = 0.2f;


    public void NewHolePosition(Transform holeTransform)
    {
        bool activated = false ;
        rowNumber = holeId / noOfColumns;
        if (Random.Range(0f,1) < chance)
        {
            activated = true;
            float randomX = startX + (column % noOfColumns );
            randomX += Random.Range(-xRange, xRange);
            float randomY = startY + (rowNumber * 1.4f);
            randomY += Random.Range(-yRange, yRange);
            // Debug.Log(randomX + " " + randomY);
            holeTransform.position = new Vector3(randomX, randomY, 0);
        }
        holeId++;
        column++;
        if (!activated)
        {
            NewHolePosition(holeTransform);
        }
    }
}
