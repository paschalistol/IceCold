using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusHole : Hole
{
    public static int bonusNumber = 0;
    [SerializeField] private TMP_Text label;
    private bool activeGoal = false;

    private void Start()
    {
        SetLabel();
    }
    protected override void EndRound()
    {
        base.EndRound();
    }
    private void SetActiveGoal(bool goal)
    {
        activeGoal = goal;
    }
    private void SetLabel()
    {
        label.SetText("-" + ++bonusNumber + "-");
    }
}
