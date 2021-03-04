using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public abstract class ActionEffect: MonoBehaviour
    {
        public virtual void Process(Tile casterTile, Tile targetTile, Action action)
        {
            if (action.isTileEffect)
            {
                targetTile.SetTileEffect(action.tileEffect);
                Instantiate(action.tileEffect.effectPrefab, targetTile.WorldPosition, Quaternion.identity);
            }

        }
    }
}

