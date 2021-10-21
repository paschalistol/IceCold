using System;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private SurvivalPool pool;
    private bool survival;
    private PoolObject type = PoolObject.BIG_HOLE;
    private HoleGeneratorSurvival survivalFunctions;
    private float distanceAfterDying = 0.7f;
    
    public virtual void BallInHole()
    {
        ClassicGameManager.instance.Die();
    }

    private void Start()
    {
        ClassicGameManager.instance.endRound += CheckDistanceAfterDying;
    }

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
        if (ClassicGameManager.instance.GetAllowControls() && ClassicGameManager.instance.GetGameMode() == GameMode.survival &&  Ball.instance.transform.position.y - transform.position.y > 4 )
        {
            survivalFunctions.NewHolePosition(transform);
            //Move Hole To new position
        }
    }

    private void OnDestroy()
    {
        ClassicGameManager.instance.endRound -= CheckDistanceAfterDying;
    }

    private void CheckDistanceAfterDying()
    {
        if (ClassicGameManager.instance.GetGameMode() == GameMode.survival && transform.position.y - distanceAfterDying < ClassicGameManager.instance.BallStartHeight)
        {
            survivalFunctions.NewHolePosition(transform);
        }
    }
}
