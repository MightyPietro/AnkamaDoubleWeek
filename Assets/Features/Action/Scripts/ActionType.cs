using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    [System.Flags]
    public enum ActionType
    {
        Attract = (1 << 0),
        Charge = (1 << 1),
        Teleportation = (1 << 2),
        Damage = (1 << 3),
        Push = (1 << 4),        
        Water = (1 << 5),
        Fire = (1 << 6),
        Earth = (1 << 7),
        Air = (1 << 8),
        DestroyTerraforming = (1 << 9),        
    }
}

