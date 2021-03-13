using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace WeekAnkama
{
    public class Damage : ActionEffect
    {
        public override bool Process(Tile casterTile, Tile targetTile, Action action)
        {
            if(targetTile.Player != null)
            {
                casterTile.Player.transform.DOLookAt(targetTile.WorldPosition, .1f);
                targetTile.Player.transform.DOLookAt(casterTile.WorldPosition, .1f);
                casterTile.Player.DoDamage(targetTile.Player, targetTile.Player.TakeDamage(casterTile.Player, action.fatigueDmg));
                return true;
            }
            return false;
        }

    }    
}

