using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    [System.Serializable]
    public abstract class TileEffect : ScriptableObject
    {
        [SerializeField] protected bool _walkable;
        [SerializeField] protected bool _crossable;

        public bool Walkable => Walkable;
        public bool Crossable => Crossable;

        public abstract void Process(Player _player);
    }
}

