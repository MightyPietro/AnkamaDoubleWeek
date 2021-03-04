using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Tile : IHeapItem<Tile>
    {
        private Grid _grid;
        private Vector2Int _coords;
        private bool _walkable;
        private TileEffect _effect;
        private Player _player;
        private Vector3 _worldPosition;

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
                    return _walkable && _effect.Walkable;
                }
                else
                    return _walkable;
            }
        }

        public bool Crossable
        {
            get
            {
                if(_effect != null)
                {
                    return _effect.Crossable;
                }
                else
                {
                    return Walkable;
                }

            }
        }

        public Player Player => _player;
        public TileEffect Effect => _effect;
        public Vector3 WorldPosition => _worldPosition;

        public event System.Action<Player> OnEnterCase;
        public event System.Action<Player> OnLeaveCase;

        public Tile(Grid grid, Vector2Int coords, Vector3 worldPosition)
        {
            _grid = grid;
            _coords = coords;
            _worldPosition = worldPosition;
        }


        public void SetPlayer(Player player)
        {
            if (player == null) return;
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
            _effect = effect;
            effect.BootUp(this);
            //OnEnterCase?.Invoke(_player);
        }

        public void UnSetTileEffect(TileEffect effect)
        {
            _effect.ShutDown();
            _effect = null;
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

