using System;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public virtual void BallInHole()
    {
        ClassicGameManager.instance.Die();
    }

}
