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
        private bool _walkable = true;
        private TileEffect _effect;
        private Player _player;
        private Vector3 _worldPosition;

        //Pathfinding
        public int gCost;
        public int hCost;
        public Tile parent;
        int heapIndex;

        public Vector2Int Coords => _coords;

        public bool Walkable => _walkable;
        public Player Player => _player;
        public Vector3 WorldPosition => _worldPosition;

        public event System.Action<Player> OnEnterCase;
        public event System.Action<Player> OnLeaveCase;

        public Tile(Grid grid, Vector2Int coords, Vector3 worldPosition)
        {
            _grid = grid;
            _coords = coords;
            _worldPosition = worldPosition;

            OnEnterCase += PerformEffect;
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

        private void PerformEffect(Player player)
        {
            _effect.Process(player);
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

