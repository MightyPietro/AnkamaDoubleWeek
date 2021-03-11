using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Fire : ActionEffect
    {
        public override bool Process(Tile casterTile, Tile targetTile, Action action)
        {
            base.Process(casterTile, targetTile, action);
            if(targetTile.Player != null)
            {
                targetTile.Player.fatigue += action.fatigueDmg;
            }
            return true;

        }
    }

}

