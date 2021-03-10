using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class WindClassPassive : PlayerPassive
    {
        public override PassiveTrigger Trigger => PassiveTrigger.BeginTurn;

        public override void ApplyPassive(Player caster, Player target)
        {
            caster.PM+=1;
        }
    }
}
