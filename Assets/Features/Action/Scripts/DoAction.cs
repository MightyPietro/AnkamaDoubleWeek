using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace WeekAnkama
{
    public class DoAction : MonoBehaviour
    {
        [SerializeField] private Action action;
        [Button]

        public void Boom()
        {
            action.Process();
        }
    }
}

