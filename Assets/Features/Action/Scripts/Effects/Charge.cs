using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Charge : ActionEffect
    {
        public override bool Process(Tile casterTile, Tile targetTile, Action action)
        {
            if (casterTile.Player != null)
            {
                Vector2 direction = (targetTile.Coords - casterTile.Coords);
                float distance = direction.magnitude;
                direction = direction.normalized;

                GlobalManager.instance.AskPushPlayer(casterTile.Player, new Vector2Int((int)direction.x, (int)direction.y), (int)distance);
                return true;
            }
            return false;
        }
    }
}

