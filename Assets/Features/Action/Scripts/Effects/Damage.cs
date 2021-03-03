using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Damage : ActionEffect
    {
        public override void Process(Tile targetTile, Action action)
        {
            Debug.Log(targetTile._player.fatigue += action.fatigueDmg);
        }

    }
}

