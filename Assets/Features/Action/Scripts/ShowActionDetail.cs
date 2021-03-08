using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WeekAnkama
{
    public class ShowActionDetail : MonoBehaviour
    {
        public void DisplayDetail(int actionIndex)
        {
            PlayerManager.instance.DisplaySpellDetail(actionIndex);
        }

        public void HideDetail()
        {
            PlayerManager.instance.HideSpellDetail();
        }
    }
}