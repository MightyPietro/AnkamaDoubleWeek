using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<T> where T: ITile
{
    private T[,] _tiles;
    private int _width;
    private int _height;
    private float _cellSize;

    public Grid(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _tiles = new T[_width, _height];
    }

    public void DebugGrid()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Debug.DrawLine(new Vector3(x , 0, y ) * _cellSize, new Vector3(x, 0, y + 1) * _cellSize);
                Debug.DrawLine(new Vector3(x , 0, y ) * _cellSize, new Vector3(x + 1, 0, y) * _cellSize);
            }
        }
        Debug.DrawLine(new Vector3(_width, 0, 0) * _cellSize, new Vector3(_width, 0, _height) * _cellSize);
        Debug.DrawLine(new Vector3(0, 0, _height) * _cellSize, new Vector3(_width, 0, _height) * _cellSize);
    }
}
