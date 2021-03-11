using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    [System.Serializable]
    public abstract class TileEffect : ScriptableObject
    {
        [SerializeField] protected bool TriggerOnEnterCase;
        [SerializeField] protected bool TriggerOnLeaveCase;
        [SerializeField] protected bool TriggerOnTurnStart;
        [SerializeField] protected bool TriggerOnTurnEnd;

        protected List<Tile> _linkedTiles = new List<Tile>();
        protected Tile _casterTile;

        [SerializeField] protected bool _walkable;
        [SerializeField] protected bool _crossable;

        public bool Walkable => _walkable;
        public bool Crossable => _crossable;

        public List<Tile> linkedTile { get { return _linkedTiles; } set { _linkedTiles = value; } }
        public Tile casterTile { get { return _casterTile; } set { _casterTile = value; } }

        public abstract void Process(Player _player);

        public virtual void BootUp(Tile tile)
        {
            if (!_linkedTiles.Contains(tile))
            {
                 _linkedTiles.Add(tile);
            }
               
            Debug.Log("Bootup");
            if (TriggerOnEnterCase) { tile.OnEnterCase += Process; }
            if (TriggerOnLeaveCase) { tile.OnLeaveCase += Process; }
            if (TriggerOnTurnStart) { tile.OnBeginTurn += Process; }
            if (TriggerOnTurnEnd) { tile.OnEndTurn += Process; }
        }

        public void ShutDown(Tile tile)
        {
            if (_linkedTiles.Count <= 0) return;

            Tile linked = _linkedTiles[_linkedTiles.IndexOf(tile)];
            Debug.Log("Shutdown");
            if (TriggerOnEnterCase) { linked.OnEnterCase -= Process; }
            if (TriggerOnLeaveCase) { linked.OnLeaveCase -= Process; }
            if (TriggerOnTurnStart) { linked.OnBeginTurn -= Process; }
            if (TriggerOnTurnEnd) { linked.OnEndTurn -= Process; }
            _linkedTiles.Remove(tile);
            casterTile = null;
        }
    }
}

