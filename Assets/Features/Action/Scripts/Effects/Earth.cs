using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Earth : ActionEffect
    {
        public override bool Process(Tile casterTile, Tile targetTile, Action action)
        {
            if(targetTile.Player != null)
            {
                //casterTile.Player.DoDamage(targetTile.Player, targetTile.Player.TakeDamage(casterTile.Player, action.fatigueDmg));
                casterTile.Player.DoDamage(targetTile.Player, targetTile.Player.TakeDamage(casterTile.Player, 80));
                FeedbackManager.instance.EarthFeedback(targetTile.WorldPosition, 2f,true);
                return true;
            }
            else
            {
                return base.Process(casterTile, targetTile, action);                
            }
        }
    }
}

