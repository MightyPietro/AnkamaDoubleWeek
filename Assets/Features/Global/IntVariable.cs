using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    [CreateAssetMenu(menuName = "Atom Variables/Int Variable")]
    public class IntVariable : ScriptableObject
    {
        public int Value;

        public void SetValue(int value) => Value = value;
    }
}

