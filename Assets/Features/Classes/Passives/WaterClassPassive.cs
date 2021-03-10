using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama {
    public class WaterClassPassive : PlayerPassive
    {
        public override PassiveTrigger Trigger => PassiveTrigger.PassEnemyExhaust;

        public override void ApplyPassive(Player caster, Player target)
        {
            caster.stockPA += 1;
        }
    }
}
