using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Grid<T>
    {
        private T[,] _tiles;
        private int _width;
        private int _height;
        private Vector2 _cellSize;
        private Vector3 _originOffset;

        public int Width => _width;
        public int Heigth => _height;
        public Vector2 CellSize => _cellSize;

        public Grid(int width, int height, Vector2 cellSize, Func<Grid<T>, Vector2Int, T> gridObjectFactory, Vector2 normalizedOffset)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _originOffset = new Vector3(normalizedOffset.x * _cellSize.x * _width, normalizedOffset.y * _cellSize.y * _height);
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
            float worldX = x * _cellSize.x + _originOffset.x;
            float worldY = y * _cellSize.y + _originOffset.y;
            return new Vector3(worldX, 0,worldY);
            
            //return TwoDToIso(x,y) + _originOffset;
        }

        public Vector3 GetTileCenterWorldPosition(int x, int y)
        {
            return GetTileWorldPosition(x,y) + new Vector3(0.5f * _cellSize.x, 0, 0.5f * _cellSize.y);
            //return GetTileWorldPosition(x, y) + TwoDToIso(0.5f, 0.5f);
        }

        #region PRIVATE METHODS
        private Vector2Int GetXY(Vector3 worldposition)
        {
            int x = Mathf.FloorToInt((worldposition.x - _originOffset.x) / _cellSize.x);
            int y = Mathf.FloorToInt((worldposition.z - _originOffset.y) / _cellSize.y);
            return new Vector2Int(x,y);
            
            //return IsoToTwoD(worldposition);
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

        private Vector3 TwoDToIso(float x, float y)
        {
            return new Vector3(x * (_cellSize.x / 2.0f) - y * (_cellSize.x / 2.0f), y * (_cellSize.y / 2.0f) + x * (_cellSize.y / 2.0f));
        }

        private Vector2Int IsoToTwoD(Vector3 worldIsoPos)
        {
            int x = Mathf.FloorToInt( ((worldIsoPos.x - _originOffset.x) / (_cellSize.x / 2.0f) + (worldIsoPos.y - _originOffset.y) / (_cellSize.y / 2.0f)) / 2.0f );
            int y = Mathf.FloorToInt( ((worldIsoPos.y - _originOffset.y) / (_cellSize.y / 2.0f) - ((worldIsoPos.x - _originOffset.x) / (_cellSize.x / 2.0f))) / 2.0f );
            return new Vector2Int( x, y);
        }

        #endregion


        #region DEBUG
        public void DebugGrid()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    Debug.DrawLine(GetTileWorldPosition(x, y), GetTileWorldPosition(x, y + 1),Color.red);
                    Debug.DrawLine(GetTileWorldPosition(x, y), GetTileWorldPosition(x + 1, y), Color.red);
                }
            }
            Debug.DrawLine(GetTileWorldPosition(_width, 0), GetTileWorldPosition(_width, _height), Color.red);
            Debug.DrawLine(GetTileWorldPosition(0, _height), GetTileWorldPosition(_width, _height), Color.red);
        }
        #endregion

        #region PATHFINDING
        /*public List<Tile> GetNeighbours(Tile tile)
        {
            List<Tile> neighbours = new List<Tile>();

            for(int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if((x== 0 || y==0) && x!=y)
                    {
                        int checkX = tile.Coords.x + x;
                        int checkY = tile.Coords.y + y;

                        if(checkX>= 0 && checkX < Width && checkY>=0 && checkY < Heigth)
                        {
                            neighbours.Add(_tiles[checkX, checkY]);
                        }
                    }
                }
            }
        }*/
        #endregion

    }
}

