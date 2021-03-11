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

            if (action.canTerraform)
            {
                targetTile.SetTileEffect(action.ActionTileEffect);
                action.ActionTileEffect.linkedTile = targetTile;
                action.ActionTileEffect.casterTile = casterTile;
            }
            return true;

        }
    }
}

