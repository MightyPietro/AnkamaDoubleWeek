using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Damage : ActionEffect
    {
        public override void Process(Tile casterTile, Tile targetTile, Action action)
        {
            if (targetTile.Player != null)
            {
                targetTile.Player.fatigue += action.fatigueDmg;
                return;
            }
            return;
            
        }

    }
}

