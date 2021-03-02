using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile
{
    Grid<ITile> Grid { get; }
    Vector2Int Coordinate { get; }
}
