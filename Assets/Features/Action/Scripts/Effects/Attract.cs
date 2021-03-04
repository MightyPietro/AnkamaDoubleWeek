﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Attract : ActionEffect
    {
        public override void Process(Tile casterTile, Tile targetTile, Action action)
        {
            if (targetTile.Player != null)
            {
                Vector2 direction = (casterTile.Coords - targetTile.Coords);
                direction = direction.normalized;

                GlobalManager.instance.AskPushPlayer(targetTile.Player, new Vector2Int((int)direction.x,(int)direction.y), action.pushCase + Mathf.FloorToInt(targetTile.Player.fatigue));
            }
        }
    }
}

