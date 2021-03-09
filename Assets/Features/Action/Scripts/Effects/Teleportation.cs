using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Teleportation : ActionEffect
    {
        public override bool Process(Tile casterTile, Tile targetTile, Action action)
        {
            if (targetTile.Walkable)
            {
                Player p = casterTile.Player;
                casterTile.UnSetPlayerNoTrigger();
                return PlayerManager.instance.TeleportPlayer(p, targetTile.Coords);
            }
            return false;
        }
    }
}

