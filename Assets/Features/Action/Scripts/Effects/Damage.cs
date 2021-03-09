using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Damage : ActionEffect
    {
        public override bool Process(Tile casterTile, Tile targetTile, Action action)
        {
            if(targetTile.Player != null)
            {
                targetTile.Player.TakeDamage(casterTile.Player, action.fatigueDmg);
                return true;
            }
            return false;
        }

    }    
}

