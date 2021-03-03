using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public abstract class ActionEffect
    {
        public virtual void Process(Tile targetTile, Action action) { Debug.Log("Action"); }
    }
}

