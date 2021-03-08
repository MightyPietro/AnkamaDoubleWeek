using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Water : ActionEffect
    {
        public override bool Process(Tile casterTile, Tile targetTile, Action action)
        {
            return base.Process(casterTile, targetTile, action);
        }
    }
}

