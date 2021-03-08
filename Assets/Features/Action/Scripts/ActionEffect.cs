using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public abstract class ActionEffect: MonoBehaviour
    {
        /*[SerializeField] protected uint _priority;
        public uint Priority => _priority;*/

        public virtual bool Process(Tile casterTile, Tile targetTile, Action action)
        {

            if (action.isTileEffect)
            {
                targetTile.SetTileEffect(action.tileEffect);
                action.tileEffect.linkedTile = targetTile;
                action.tileEffect.casterTile = casterTile;
                return true;
            }
            return false;

        }
    }
}

