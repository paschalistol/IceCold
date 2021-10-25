using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalHole : Hole
{
    private HoleGeneratorSurvival survivalFunctions;
    private float distanceAfterDying = 0.7f;
    private Animator anim;
    private Vector3 localScale;
    void Start()
    {
        ClassicGameManager.instance.endRound += CheckDistanceAfterDying;
        anim = GetComponent<Animator>();
    }
    private void CheckDistanceAfterDying()
    {
        if (ClassicGameManager.instance.GetGameMode() == GameMode.survival && transform.position.y - distanceAfterDying < ClassicGameManager.instance.BallStartHeight)
        {
            localScale = transform.localScale;
            anim.SetTrigger("CloseHole");
        }
    }

    private void MoveHoleToNew()
    {
        survivalFunctions.NewHolePosition(transform);
        transform.localScale = localScale;
    }

    void Update()
    {
        if (ClassicGameManager.instance.GetAllowControls() &&  Ball.instance.transform.position.y - transform.position.y > 4 )
        {
            survivalFunctions.NewHolePosition(transform);
            //Move Hole To new position
        }
    }
    private void OnDestroy()
    {
        ClassicGameManager.instance.endRound -= CheckDistanceAfterDying;
    }
    public void SurvivalHoleInit(SurvivalPool survivalPool)
    {
        if (survivalFunctions == null)
        {
            survivalFunctions = survivalPool.gameObject.GetComponent<HoleGeneratorSurvival>();
        }
        localScale = transform.localScale;
        MoveHoleToNew();
    }
}
