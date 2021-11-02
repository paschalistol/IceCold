using UnityEngine;

public class SurvivalPowerUp : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private GameObject face;
    private PoolObject type;
    private SurvivalPool survivalPool;
    private bool initialized = false;

    public void PowerUpInit(PoolObject poolObject, SurvivalPool pool)
    {
        if (!initialized)
        {
            survivalPool = pool;
            type = poolObject;
            initialized = true;
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
