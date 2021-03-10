using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class FireClassPassive : PlayerPassive
    {
        public override PassiveTrigger Trigger => PassiveTrigger.DoDamage;

        public override void ApplyPassive(Player caster, Player target)
        {
            target.TakeDamage(caster, 20);
        }
    }
}
