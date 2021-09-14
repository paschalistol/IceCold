using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusHole : Hole
{
    public static int bonusNumber = 0;
    [SerializeField] private TMP_Text label;
    private bool activeGoal = false;
    Animation anim;


    private void Start()
    {
        SetLabel();
        anim = GetComponent<Animation>();
        SetActiveGoal(false);
        ClassicGameManager.instance.AddBonus(this);
    }
    public override void BallInHole()
    {
        if (activeGoal)
        {
            ClassicGameManager.instance.BallInBonus();
        }
        else
        {
            base.BallInHole();
        }
    }
    public void SetActiveGoal(bool goal)
    {
        activeGoal = goal;
        PlayAnimation(goal);
    }
    private void SetLabel()
    {
        label.SetText("-" + ++bonusNumber + "-");
    }
    private void PlayAnimation(bool play)
    {
        if (play)
        {
            anim.Play();
        }
        else
        {
            foreach (AnimationState state in anim)
            {
                state.time = 0f;
            }
            anim.Sample();
            anim.Stop();
        }
    }
}
