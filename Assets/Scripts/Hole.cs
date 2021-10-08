using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private SurvivalPool pool;
    private bool survival;
    private PoolObject type = PoolObject.BIG_HOLE;

    public virtual void BallInHole()
    {
        ClassicGameManager.instance.Die();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pool") && survival)
        {
            gameObject.SetActive(false);
            pool.AddToPool(type, gameObject);
        }
    }
    public void SurvivalHole(SurvivalPool survivalPool)
    {
        pool = survivalPool;
        survival = true;
    }
}
