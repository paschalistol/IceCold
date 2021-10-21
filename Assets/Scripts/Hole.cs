using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private SurvivalPool pool;
    private bool survival;
    private PoolObject type = PoolObject.BIG_HOLE;
    private HoleGeneratorSurvival survivalFunctions;
    
    public virtual void BallInHole()
    {
        ClassicGameManager.instance.Die();
    }
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Pool") && survival)
    //     {
    //         gameObject.SetActive(false);
    //         pool.AddToPool(type, gameObject);
    //     }
    // }
    public void SurvivalHole(SurvivalPool survivalPool)
    {
        pool = survivalPool;
        survival = true;
        if (survivalFunctions == null)
        {
            survivalFunctions = survivalPool.gameObject.GetComponent<HoleGeneratorSurvival>();
        }
        survivalFunctions.NewHolePosition(transform);
    }
    private void Update()
    {
        if (ClassicGameManager.instance.GetAllowControls() && ClassicGameManager.instance.GetGameMode() == GameMode.survival &&  Ball.instance.transform.position.y - transform.position.y > 2 )
        {
            survivalFunctions.NewHolePosition(transform);
            //Move Hole To new position
        }
    }
}
