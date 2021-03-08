using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace WeekAnkama
{
    [CreateAssetMenu(menuName = "Assets/ActionList")]
    public class ActionsList : ScriptableObject
    {
        public List<Action> Value;

        public void SetValue(List<Action> value) => Value = value;

        [Button]
        public void GetReferences()
        {
            Value.Clear();
            foreach (Action action in Object.FindObjectsOfTypeIncludingAssets(typeof(Action)))
            {
                Value.Add(action);
            }
            SetDirty();

        }
    }
}

