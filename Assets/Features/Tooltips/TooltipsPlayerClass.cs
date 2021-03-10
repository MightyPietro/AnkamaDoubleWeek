using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class TooltipsPlayerClass : TooltipsBehavior
    {
        private IntVariable playerData;

        public override void ShowTooltip()
        {
            TooltipsManager.ShowTooltip(playerData.name, playerData.name);
        }
    }
}
