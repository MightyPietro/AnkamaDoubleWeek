using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class DestroyTerraforming : ActionEffect
    {
        public override bool Process(Tile casterTile, Tile targetTile, Action action)
        {
            if(targetTile != null && targetTile.Effect != null)
            {
                targetTile.UnSetTileEffect();
                return true;
            }
            return false;
        }
    }
}

