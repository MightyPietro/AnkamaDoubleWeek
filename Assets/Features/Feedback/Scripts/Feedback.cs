using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    [CreateAssetMenu(menuName = "Assets/Feedback")]
    public class Feedback : ScriptableObject
    {
        public GameObject VFX;
        public AudioClip[] clips;
    }

}
