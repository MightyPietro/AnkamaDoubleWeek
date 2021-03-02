using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : ITile
{
    private Grid<ITile> _grid;
    private readonly Vector2Int _coordinate;

    public Grid<ITile> Grid => _grid;

    public Vector2Int Coordinate 
    {
        get
        {
            return _coordinate;
        }
    }

    public Tile(Vector2Int coordinate, Grid<ITile> grid)
    {
        _grid = grid;
        _coordinate = coordinate;
    }

    public Tile(int x, int y, Grid<ITile> grid) : this(new Vector2Int(x,y),grid)
    {
    }
}
