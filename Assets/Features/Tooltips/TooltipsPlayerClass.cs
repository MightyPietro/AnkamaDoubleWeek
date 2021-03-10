using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class TooltipsPlayerClass : TooltipsBehavior
    {
        [SerializeField]
        private IntVariable playerData;

        public override void ShowTooltip()
        {
            TooltipsManager.ShowTooltip(playerData.playerClass.nom, playerData.playerClass.description);
        }
    }
}
