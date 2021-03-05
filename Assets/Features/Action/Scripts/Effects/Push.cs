using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{

    public class Push : ActionEffect
    {
        public override void Process(Tile casterTile, Tile targetTile, Action action)
        {
            if (targetTile.Player != null)
            {
                Vector2 direction = (targetTile.Coords - casterTile.Coords);
                direction = direction.normalized;

                GlobalManager.instance.AskPushPlayer(targetTile.Player, new Vector2Int((int)direction.x, (int)direction.y), action.pushCase + Mathf.FloorToInt(targetTile.Player.fatigue/100));
            }
        }

    }
}

