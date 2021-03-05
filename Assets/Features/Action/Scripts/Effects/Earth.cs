using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Earth : ActionEffect
    {
        public override void Process(Tile casterTile, Tile targetTile, Action action)
        {
            base.Process(casterTile, targetTile, action);

            if(targetTile.Player != null)
            {
                targetTile.Player.fatigue += action.fatigueDmg;
                action.tileEffect.linkedTile.effectVisual.ShutDown();
            }


        }
    }
}

