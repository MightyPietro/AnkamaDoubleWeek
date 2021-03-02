using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Bootstrapper : MonoBehaviour
    {
        Grid<Tile> _grid;
        public int x = 10;
        public int y = 10;
        public float size;
        public GameObject test;
        public GameObject pointer;

        public Vector3 pos;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        private void Update()
        {
            pos = pointer.transform.position;
            pos.z = 0;
            _grid = new Grid<Tile>(x, y, size, (grid, coords) => { return new Tile(grid,coords); }, new Vector3(- x * size *0.5f, -y* size*0.5f));
            _grid.DebugGrid();

            if(_grid.TryGetTile(pos, out Tile tile))
                test.transform.position = _grid.GetTileCenterWorldPosition(tile.Coords.x, tile.Coords.y);
        }
    }
}

