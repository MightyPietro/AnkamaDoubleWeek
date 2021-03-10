using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WeekAnkama
{
    public class ShowActionDetail : MonoBehaviour
    {
        [SerializeField]
        private GameObject vfx;

        public void DisplayDetail(int actionIndex)
        {
            PlayerManager.instance.DisplaySpellDetail(actionIndex);
            vfx.SetActive(true);
        }

        public void HideDetail()
        {
            PlayerManager.instance.HideSpellDetail();
            vfx.SetActive(false);
        }
    }
}