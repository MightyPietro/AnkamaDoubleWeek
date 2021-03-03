using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    [System.Serializable]
    [CreateAssetMenu(fileName ="FireTileEffect", menuName = "CustomAssets/FireTileEffect")]
    public class FireTileEffect : TileEffect
    {
        [SerializeField]protected int _fatigue;

        public override void Process(Player _player)
        {
            _player.fatigue += _fatigue;
        }
    }
}

