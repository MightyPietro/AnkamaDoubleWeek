using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "FireTileEffect", menuName = "Assets/TileEffects/Water")]
    public class WaterTileEffect : TileEffect
    {
        [SerializeField] protected int _fatigue;

        public override void Process(Player _player)
        {
            if (_player == null)
            {
                Debug.LogError($"Player is Null in { this.GetType().ToString()}");
                return;
            }

            _player.PM--;
            FeedbackManager.instance.WaterFeedback(GridManager.Grid.GetTileWorldPosition(_player.position.x, _player.position.y), 1.5f);
        }

    }
}

