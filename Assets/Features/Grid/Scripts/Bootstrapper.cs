using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WeekAnkama
{
    public class Bootstrapper : MonoBehaviour
    {
        public Grid _grid;
        public int x = 10;
        public int y = 10;
        public Vector2 size;
        public GameObject test;
        public GameObject pointer;
        public Text text;
        Tile currentTile;
        public Vector3 pos;

        // Start is called before the first frame update
        void Awake()
        {
            MouseHandler.OnMouseMove += (Vector2 position) => {
                RaycastHit hitData;
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(position.x, position.y)); 
                if(Physics.Raycast(ray,out hitData, 100))
                {
                    pos = hitData.point;
                }
            };

            MouseHandler.OnMouseLeftClick += () =>
            {
                Tile oldTile = currentTile;
                if (_grid.TryGetTile(pos, out currentTile))
                {
                    text.transform.position = Camera.main.WorldToScreenPoint(_grid.GetTileWorldPosition(currentTile.Coords.x, currentTile.Coords.y));
                    test.transform.position = _grid.GetTileWorldPosition(currentTile.Coords.x, currentTile.Coords.y);                    
                }
                else
                {
                    currentTile = oldTile;
                }
            };
            _grid = new Grid(x, y, size, (grid, coords) => { return new Tile(grid, coords); }, new Vector2(-0.5f, -0.5f));
        }

        private void Update()
        {

            
            _grid.DebugGrid();


            if (currentTile == null) return;
            currentTile.value++;
            text.text = $"{currentTile.Coords.x} - {currentTile.Coords.y} ____ { currentTile.value.ToString()}";

        }
    }
}

