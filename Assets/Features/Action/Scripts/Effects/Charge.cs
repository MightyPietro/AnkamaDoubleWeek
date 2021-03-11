using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
                casterTile.Player.transform.DOLookAt(targetTile.WorldPosition, .1f);
                targetTile.Player.transform.DOLookAt(casterTile.WorldPosition, .1f);
                casterTile.Player.anim.SetBool("isRun", true);

                GlobalManager.instance.AskPushPlayer(casterTile.Player, new Vector2Int((int)direction.x, (int)direction.y), (int)distance);


                return true;
            }
            return false;
        }
    }
}

