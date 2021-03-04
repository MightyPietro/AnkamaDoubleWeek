using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Water : ActionEffect
    {
        public override void Process(Tile targetTile, Action action)
        {
            Instantiate(action.prefab, targetTile.WorldPosition,Quaternion.identity);
        }
    }
}

