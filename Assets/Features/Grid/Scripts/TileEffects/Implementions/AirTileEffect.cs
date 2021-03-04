using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "FireTileEffect", menuName = "Assets/TileEffects/Air")]
    public class AirTileEffect : TileEffect
    {
        [SerializeField] protected int _fatigue;

        public override void Process(Player _player)
        {
            if (_player == null)
            {
                Debug.LogError($"Player is Null in { this.GetType().ToString()}");
                return;
            }
            
            // Push le player d'une case par rapport à d'où il arrive
        }

    }
}