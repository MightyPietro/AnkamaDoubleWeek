﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Tile : IHeapItem<Tile>
    {
        private Grid _grid;
        private Vector2Int _coords;
        private bool _walkable = true;
        private bool _crossable = true;
        private TileEffect _effect;
        private Player _player;
        private Vector3 _worldPosition;
        private TileEffectDisplay _effectVisual;

        //Pathfinding
        public int gCost;
        public int hCost;
        public Tile parent;
        int heapIndex;

        public Vector2Int Coords => _coords;

        public bool Walkable {
            get
            {
                if (_effect != null)
                {
                    return _walkable && _effect.Walkable && _player == null;
                }
                else
                    return _walkable && _player == null;
            }
        }

        public bool Crossable
        {
            get
            {
                if(_effect != null)
                {
                    Debug.Log(Effect.Crossable);
                    return _crossable && _effect.Crossable;// && _player == null;
                }
                else
                {
                    return _crossable && _player == null;
                }

            }
        }
        public TileEffectDisplay effectVisual => _effectVisual;
        public bool Usable { get; set; } = true;

        public Player Player => _player;
        public TileEffect Effect => _effect;
        public Vector3 WorldPosition => _worldPosition;

        public event System.Action<Player> OnEnterCase;
        public event System.Action<Player> OnLeaveCase;
        public event System.Action<Player> OnBeginTurn;
        public event System.Action<Player> OnEndTurn;

        public Tile(Grid grid, Vector2Int coords, Vector3 worldPosition)
        {
            _grid = grid;
            _coords = coords;
            _worldPosition = worldPosition;
            _walkable = true;

            TurnManager.OnBeginPlayerTurn += HandleBeginTurn;
            TurnManager.OnEndPlayerTurn += HandleEndTurn;
        }


        public void SetPlayer(Player player)
        {
            if (player == null || player == _player) return;
            _player = player;
            OnEnterCase?.Invoke(player);
        }

        public void UnSetPlayer()
        {            
            OnLeaveCase?.Invoke(_player);
            _player = null;
        }

        public void SetTileEffect(TileEffect effect)
        {
            if(effect == null)
            {
                Debug.LogError("Impossible to set effect, effect is Null !!!");
                return;
            }
            if (_effect == null)
            {
                _effectVisual = TileEffectPool.Instance.GetObjectInPool();
            }
            _effect = effect;
            effect.BootUp(this);
            //OnEnterCase?.Invoke(_player);            
            _effectVisual.BootUp(WorldPosition, effect);
        }

        public void UnSetTileEffect()
        {
            if (_effect != null)
            {
                _effect.ShutDown();
                _effect = null;
                _effectVisual.ShutDown();
                TileEffectPool.Instance.ReturnObject(_effectVisual);
                _effectVisual = null;
            }
        }

        private void HandleBeginTurn(Player player)
        {
            if (player != _player) return;
            OnBeginTurn?.Invoke(_player);
        }

        private void HandleEndTurn(Player player)
        {
            if (player != _player) return;
            OnEndTurn?.Invoke(_player);
        }



        #region Pathfinding

        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }
        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public int CompareTo(Tile nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }
            return -compare;
        }
        #endregion
    }
}

