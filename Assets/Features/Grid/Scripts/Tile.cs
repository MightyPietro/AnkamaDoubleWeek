using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Tile
    {
        private Grid<Tile> _grid;
        private Vector2Int _coords;

        public Vector2Int Coords => _coords;

        public Tile(Grid<Tile> grid, Vector2Int coords)
        {
            _grid = grid;
            _coords = coords;
        }

    }
}

