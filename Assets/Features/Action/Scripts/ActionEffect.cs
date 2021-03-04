using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public abstract class ActionEffect: MonoBehaviour
    {
        protected GameObject instantiatedPrefab;
        public virtual void Process(Tile casterTile, Tile targetTile, Action action)
        {
            if (action.isTileEffect && targetTile.Effect == null)
            {
                targetTile.SetTileEffect(action.tileEffect);
                instantiatedPrefab = Instantiate(action.tileEffect.effectPrefab, targetTile.WorldPosition, Quaternion.identity);
            }

        }
    }
}

