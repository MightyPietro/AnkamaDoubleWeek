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

            Vector2 direction = (linkedTile.Coords - casterTile.Coords);
            direction = direction.normalized;

            GlobalManager.instance.AskPushPlayer(_player, new Vector2Int((int)direction.x, (int)direction.y), 1 + Mathf.FloorToInt(_player.fatigue / 100));
        }

       
    }
}