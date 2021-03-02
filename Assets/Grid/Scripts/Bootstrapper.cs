using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    Grid<Tile> _grid;
    public int x = 10;
    public int y = 10;
    public float size;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        _grid = new Grid<Tile>(x, y, size);
        _grid.DebugGrid();
    }
}
