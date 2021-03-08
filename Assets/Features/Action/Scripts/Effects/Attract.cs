using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Attract : ActionEffect
    {
        public override bool Process(Tile casterTile, Tile targetTile, Action action)
        {
            Debug.Log(targetTile.Player);
            if (targetTile.Player != null)
            {
                Vector2 direction = (casterTile.Coords - targetTile.Coords);
                direction = direction.normalized;

                GlobalManager.instance.AskPushPlayer(targetTile.Player, new Vector2Int((int)direction.x,(int)direction.y), action.pushCase + Mathf.FloorToInt(targetTile.Player.fatigue/100));
                return true;
            }
            return false;
        }
    }
}

