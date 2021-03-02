using System;
using UnityEngine;

namespace WeekAnkama
{
    public class Grid<T>
    {
        private T[,] _tiles;
        private int _width;
        private int _height;
        private float _cellSize;
        private Vector3 _originOffset;

        public int Width => _width;
        public int Heigth => _height;
        public float CellSize => _cellSize;

        public Grid(int width, int height, float cellSize, Func<Grid<T>, Vector2Int, T> gridObjectFactory, Vector3 originOffset)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _originOffset = originOffset;
            _tiles = new T[_width, _height];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _tiles[x, y] = gridObjectFactory(this, new Vector2Int(x, y));
                }
            }
        }

        /// <summary>
        /// Warning, always TRUE for non-nullable types
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="tile"></param>
        /// <returns></returns>
        public bool TryGetTile(Vector3 worldPosition, out T tile)
        {
            Vector2Int position = GetXY(worldPosition);
            tile = GetTile(position.x, position.y);
            return tile != null;
        }

        public void SetTile(Vector3 worldPosition, T tile)
        {
            Vector2Int position = GetXY(worldPosition);
            SetTile(position.x, position.y, tile);
        }

        public Vector3 GetTileWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * _cellSize + _originOffset;
        }

        public Vector3 GetTileCenterWorldPosition(int x, int y)
        {
            return GetTileWorldPosition(x,y) + new Vector3(0.5f * _cellSize, 0.5f * _cellSize) ;
        }

        #region PRIVATE METHODS
        private Vector2Int GetXY(Vector3 worldposition)
        {
            return new Vector2Int(Mathf.FloorToInt( (worldposition.x - _originOffset.x)/ _cellSize),Mathf.FloorToInt((worldposition.y - _originOffset.y) / _cellSize));
        }

        private void SetTile(int x, int y, T tile)
        {
            if (x >= 0 && y >= 0 && x < _width &&  y < _height)
            {
                _tiles[x, y] = tile;
            }
        }

        private T GetTile(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                return _tiles[x, y];
            }
            else
            {
                return default(T);
            }
        }
        #endregion


        #region DEBUG
        public void DebugGrid()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    Debug.DrawLine(GetTileWorldPosition(x, y), GetTileWorldPosition(x, y + 1));
                    Debug.DrawLine(GetTileWorldPosition(x, y), GetTileWorldPosition(x + 1, y));
                }
            }
            Debug.DrawLine(GetTileWorldPosition(_width, 0), GetTileWorldPosition(_width, _height));
            Debug.DrawLine(GetTileWorldPosition(0, _height), GetTileWorldPosition(_width, _height));
        }
        #endregion
    }
}

