using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    [System.Flags]
    public enum ActionType
    {
        Attract = (1 << 0),
        Teleportation = (1 << 1),
        Damage = (1 << 2),
        Push = (1 << 3),        
        Water = (1 << 4),
        Fire = (1 << 5),
        Earth = (1 << 6),
        Air = (1 << 7),
        DestroyTerraforming = (1 << 8),        
    }
}

