using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WeekAnkama
{
    public class TooltipsPlayerClass : TooltipsBehavior
    {
        [SerializeField]
        private IntVariable playerData;

        public override void ShowTooltip()
        {
            if(!PhotonNetwork.IsConnected)
                TooltipsManager.ShowTooltip(PlayerManager.instance.actualPlayer.classe.nom, PlayerManager.instance.actualPlayer.classe.description);
            else TooltipsManager.ShowTooltip(playerData.playerClass.nom, playerData.playerClass.description);
        }
    }
}
