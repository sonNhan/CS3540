using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public enum TargetPriority
    {
        FIRST,
        LAST,
        CLOSE
    }

    public enum UpgradeType
    {
        RANGE,
        DAMAGE,
        SPEED
    }

    public enum Ability
    {
        EXPLOSION,
        BLIZZARD
    }

    public enum AbilityEffect
    {
        DAMAGE,
        SLOW
    }
}
