using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    [CreateAssetMenu(menuName = "Assets/Action")]
    public class Action : ScriptableObject
    {
        [SerializeField, SerializeReference]

        public List<IActionEffect> actionEffects;
    }
}

