using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Attract : ActionEffect
    {
        public override void Process(Tile casterTile, Tile targetTile, Action action)
        {
            Debug.Log("Attract");
        }
    }
}

