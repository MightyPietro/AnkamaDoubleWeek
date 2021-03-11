using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    [System.Serializable]
    [CreateAssetMenu(fileName ="FireTileEffect", menuName = "Assets/TileEffects/Fire")]
    public class FireTileEffect : TileEffect
    {
        [SerializeField]protected int _fatigue;

        public override void Process(Player _player)
        {
            if (_player == null) { 
                Debug.LogError($"Player is Null in { this.GetType().ToString()}");
                return;
            }
            if(casterTile == null)
            {
                _player.TakeDamage(null, _fatigue);
            }
            else
            {
                _player.TakeDamage(casterTile.Player, _fatigue);
            }
            
            FeedbackManager.instance.CharaFireFeedback(_player.transform.position + _player.transform.localScale, 3f,_player.transform);
        }

    }
}

