using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace WeekAnkama
{
    public class TooltipsManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject tooltipsParent;
        [SerializeField]
        private TextMeshProUGUI titleTxt, descriptionTxt;

        private static TooltipsManager instance;

        private void Awake()
        {
            instance = this;
        }

        public static void ShowTooltip(string title, string description)
        {
            instance.titleTxt.text = title;
            instance.descriptionTxt.text = description;
            instance.tooltipsParent.SetActive(true);
        }

        public static void HideTooltip()
        {
            instance.tooltipsParent.SetActive(false);
        }
    }
}
