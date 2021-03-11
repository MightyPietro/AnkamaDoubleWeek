using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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

                targetTile.Player.transform.DOLookAt(casterTile.WorldPosition, .1f);
                casterTile.Player.transform.DOLookAt(targetTile.WorldPosition, .1f);
                targetTile.Player.anim.SetBool("isRun", true);
                GlobalManager.instance.AskPushPlayer(targetTile.Player, new Vector2Int((int)direction.x,(int)direction.y), action.pushCase + Mathf.FloorToInt(targetTile.Player.fatigue/100));
                return true;
            }
            return false;
        }
    }
}

