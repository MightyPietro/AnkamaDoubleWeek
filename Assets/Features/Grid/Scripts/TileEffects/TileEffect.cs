﻿using System.Collections;
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

        protected Tile _linkedTile;

        [SerializeField] protected bool _walkable;
        [SerializeField] protected bool _crossable;

        public bool Walkable => _walkable;
        public bool Crossable => _crossable;

        public abstract void Process(Player _player);

        public virtual void BootUp(Tile tile)
        {
            _linkedTile = tile;

            if (TriggerOnEnterCase) { _linkedTile.OnEnterCase += Process; }
            if (TriggerOnLeaveCase) { _linkedTile.OnEnterCase += Process; }
            if (TriggerOnTurnStart) { TurnManager.OnBeginPlayerTurn += Process; }
            if (TriggerOnTurnEnd) { TurnManager.OnEndPlayerTurn += Process; }
        }

        public void ShutDown()
        {
            if (TriggerOnEnterCase) { _linkedTile.OnEnterCase -= Process; }
            if (TriggerOnLeaveCase) { _linkedTile.OnEnterCase -= Process; }
            if (TriggerOnTurnStart) { TurnManager.OnBeginPlayerTurn -= Process; }
            if (TriggerOnTurnEnd) { TurnManager.OnEndPlayerTurn -= Process; }
        }
    }
}

