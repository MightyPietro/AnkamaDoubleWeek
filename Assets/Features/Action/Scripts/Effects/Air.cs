using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Air : ActionEffect
    {
        public override bool Process(Tile casterTile, Tile targetTile, Action action)
        {
            return base.Process(casterTile, targetTile, action);
            
        }
    }

}

