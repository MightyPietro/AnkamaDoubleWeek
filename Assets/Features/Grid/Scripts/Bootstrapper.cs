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
        public FireTileEffect effect;

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

                    MouseHandler.OnTileClick(currentTile);
                }
                else
                {
                    currentTile = oldTile;
                }
            };
            _grid = new Grid(x, y, size, (grid, coords) => { return new Tile(grid, coords, grid.GetTileWorldPosition(coords.x, coords.y)); }, new Vector2(-0.5f, -0.5f));
            _grid.TryGetTile(Vector3.zero, out Tile t);
            t.SetTileEffect(effect);

            _grid.TryGetTile(new Vector3(3,0,1), out Tile t2);
            t2.SetTileEffect(effect);
        }

        private void Update()
        {
            //MouseHandler.Instance.DisableGameplayInputs();
            _grid.DebugGrid();

            _grid.TryGetTile(Vector3.zero, out Tile t);
            t.SetTileEffect(effect);
            Debug.Log("-------------" + t.Effect.ToString()) ;

            _grid.TryGetTile(new Vector3(3, 0, 1), out Tile t2);
            t2.SetTileEffect(effect);
            Debug.Log("-------------" + t2.Effect.ToString());

            if (currentTile == null) return;
            //currentTile.value++;
            text.text = $"{currentTile.Coords.x} - {currentTile.Coords.y} ____ ";    
        }
    }
}

