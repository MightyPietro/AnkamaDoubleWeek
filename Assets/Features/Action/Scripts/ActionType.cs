using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    [System.Flags]
    public enum ActionType
    {
        Attract = (1 << 0),
        Damage = (1 << 1),
        Push = (1 << 2),        
        Water = (1 << 3),
        Fire = (1 << 4),
        Earth = (1 << 5),
        Air = (1 << 6),
        DestroyTerraforming = (1 << 7),
    }
}

