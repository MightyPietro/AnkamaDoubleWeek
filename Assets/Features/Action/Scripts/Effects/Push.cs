using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace WeekAnkama
{

    public class Push : ActionEffect
    {
        public override bool Process(Tile casterTile, Tile targetTile, Action action)
        {            
            if (targetTile.Player != null)
            {
                Vector2 direction = (targetTile.Coords - casterTile.Coords);
                direction = direction.normalized;
                FeedbackManager.instance.PushFeedback(casterTile.WorldPosition, 2f, targetTile.Player);
                casterTile.Player.transform.DOLookAt(targetTile.WorldPosition, .1f);
                targetTile.Player.transform.DOLookAt(casterTile.WorldPosition, .1f);
                GlobalManager.instance.AskPushPlayer(targetTile.Player, new Vector2Int((int)direction.x, (int)direction.y), action.pushCase + Mathf.FloorToInt(targetTile.Player.fatigue/100));


                return true;
            }
            return false;
        }

    }
}

