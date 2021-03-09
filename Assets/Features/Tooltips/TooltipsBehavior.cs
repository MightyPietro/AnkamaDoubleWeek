using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class TooltipsBehavior : MonoBehaviour
    {
        [SerializeField]
        private string titre;

        [SerializeField, TextArea(2,3)]
        private string description;

        public void ShowTooltip()
        {
            TooltipsManager.ShowTooltip(titre, description);
        }

        public void HideTooltip()
        {
            TooltipsManager.HideTooltip();
        }
    }
}
