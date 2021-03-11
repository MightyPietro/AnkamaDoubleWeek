using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public enum PlayerClasses { Fire, Water, Wind, Earth}

    [CreateAssetMenu(menuName = "PlayerClass")]
    public class PlayerClassScriptable : ScriptableObject
    {
        public string nom;
        public string description;
        public Sprite icon;

        public PlayerClasses passive;

        public List<Action> deck;

    }
}
