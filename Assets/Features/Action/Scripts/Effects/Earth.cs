using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Earth : ActionEffect
    {
        public override void Process(Tile casterTile, Tile targetTile, Action action)
        {


            if(targetTile.Player != null)
            {
                targetTile.Player.fatigue += action.fatigueDmg;

            }
            else
            {
                base.Process(casterTile, targetTile, action);
            }


        }
    }
}

