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
                return PlayerManager.instance.TeleportPlayer(p, targetTile.Coords, true);
            }
            else if(targetTile.Player != null && casterTile.Player != null)
            {
                Player pC = casterTile.Player;
                Player pT = targetTile.Player;
                PlayerManager.instance.TeleportPlayer(pC, targetTile.Coords, false);
                PlayerManager.instance.TeleportPlayer(pT, casterTile.Coords, false);
                return true;
            }
            return false;
        }
    }
}

