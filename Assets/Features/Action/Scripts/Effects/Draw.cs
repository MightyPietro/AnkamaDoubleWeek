using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Draw : ActionEffect
    {
        public override bool Process(Tile casterTile, Tile targetTile, Action action)
        {
            if(targetTile != null && targetTile.Player != null && targetTile.Player.hand.Count < 7)
            {
                PlayerManager.instance.DrawCard(targetTile.Player);
                return true;
            }
            return false;
        }
    }
}

