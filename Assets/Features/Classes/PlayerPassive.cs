using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama {
    public enum PassiveTrigger { BeginTurn, TakeDamage, DoDamage, PassEnemyExhaust, PassSelfExhaust }

    public abstract class PlayerPassive
    {
        public virtual PassiveTrigger Trigger { get; }

        public virtual void ApplyPassive(Player caster, Player target)
        {

        }
    }
}
