using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{

    public class Push : ActionEffect
    {
        public override void Process(Tile casterTile, Tile targetTile, Action action)
        {
            Debug.Log("Push");
        }

    }
}

