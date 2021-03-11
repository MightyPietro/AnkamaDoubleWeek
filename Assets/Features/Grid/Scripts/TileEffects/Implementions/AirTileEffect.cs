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
            Debug.Log("Test");

            if (_player == null)
            {
                Debug.LogError($"Player is Null in { this.GetType().ToString()}");
                return;
            }
            else
            {
                GridManager.Grid.TryGetTile(_player.position, out Tile playerTile);

                /*Vector2 direction = (linkedTile.Coords - playerTile.Coords);
                direction = direction.normalized;*/

                //linkedTile.UnSetPlayer();

                Debug.Log("Push test effect : " + _player.Direction);
                GlobalManager.instance.AskPushPlayer(_player, _player.Direction, 1 + Mathf.FloorToInt(_player.fatigue / 100));
            }
        }

       
    }
}