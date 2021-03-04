using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public abstract class ActionEffect: MonoBehaviour
    {
        public virtual void Process(Tile casterTile, Tile targetTile, Action action)
        {
            Debug.Log(action.tileEffect);

            if (action.isTileEffect)
            {
                
                targetTile.SetTileEffect(action.tileEffect);
            }

        }
    }
}

