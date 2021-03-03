using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WeekAnkama
{
    public class Bootstrapper : MonoBehaviour
    {
        Grid<Tile> _grid;
        public int x = 10;
        public int y = 10;
        public Vector2 size;
        public GameObject test;
        public GameObject pointer;
        public Text text;

        public Vector3 pos;

        // Start is called before the first frame update
        void Start()
        {
            _grid = new Grid<Tile>(x, y, size, (grid, coords) => { return new Tile(grid, coords); }, new Vector2(-0.5f, -0.5f));
        }

        private void Update()
        {
            pos = pointer.transform.position;
            pos.y = 0;
            _grid.DebugGrid();


            if (_grid.TryGetTile(pos, out Tile tile)) {
                text.transform.position = Camera.main.WorldToScreenPoint(_grid.GetTileCenterWorldPosition(tile.Coords.x, tile.Coords.y));
                test.transform.position = _grid.GetTileCenterWorldPosition(tile.Coords.x, tile.Coords.y);
                tile.value++;
                text.text = $"{tile.Coords.x} - {tile.Coords.y} ____ { tile.value.ToString()}"; 
            }
        }
    }
}

