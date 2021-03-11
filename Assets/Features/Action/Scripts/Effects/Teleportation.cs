using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
                p.transform.DOLookAt(targetTile.WorldPosition, .1f);
                p.anim.SetTrigger("Skill");
                return PlayerManager.instance.TeleportPlayer(p, targetTile.Coords, true);
            }
            else if(targetTile.Player != null && casterTile.Player != null)
            {
                Player pC = casterTile.Player;
                Player pT = targetTile.Player;
                PlayerManager.instance.TeleportPlayer(pC, targetTile.Coords, false);
                PlayerManager.instance.TeleportPlayer(pT, casterTile.Coords, false);
                pC.transform.DOLookAt(pT.transform.position, .1f);
                pC.anim.SetTrigger("Skill");
                return true;
            }


            return false;
        }
    }
}

