using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyMovement
{
    public float GetDistanceToGoal();

    public void Slow(int amount);
}
