using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class EarthClassPassive : PlayerPassive
    {
        public override PassiveTrigger Trigger => PassiveTrigger.TakeDamage;

        public override void ApplyPassive(Player caster, Player target)
        {
            target.TakeHeal(20);
        }
    }
}
