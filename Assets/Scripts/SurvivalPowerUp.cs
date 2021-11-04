using UnityEngine;

public class SurvivalPowerUp : SurvivalPoolObjectBase
{
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private GameObject face;
    
    [SerializeField] private int powerUpPoints = 2;

    public int PowerUpPoints
    {
        get
        {
            return powerUpPoints;
        }
    }
    void Update()
    {
        face.transform.eulerAngles = new Vector3(0, face.transform.eulerAngles.y + Time.deltaTime * rotationSpeed, 0);
        if (ClassicGameManager.instance.GetAllowControls() &&  Ball.instance.transform.position.y - transform.position.y > 4 )
        {
            //Add back to pool
            BackToPool();
        }
    }

        public void BackToPool()
        {
            survivalPool.AddToPool(type, gameObject);
        }
}
