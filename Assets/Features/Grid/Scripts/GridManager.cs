using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WeekAnkama
{
    public class GridManager : MonoBehaviour
    {
        private static Grid _grid;

        [SerializeField] private GridLevel _settings;
        [SerializeField] private GameObject[] _floor;
        private GameObject[,] _tilesVisual;                

        public static Grid Grid => _grid;

        private void Awake()
        {
            _grid = new Grid(_settings.Width, _settings.Heigth, _settings.CellSize, 
                (grid, coords) => { return new Tile(grid, coords, grid.GetTileWorldPosition(coords.x, coords.y)); }
                , _settings.NormalizedOffset);

            _tilesVisual = new GameObject[_grid.Width, _grid.Heigth];

            int x = 0, y = 0;
            foreach (var item in _floor)
            {
                _tilesVisual[x, y] = item;

                x++;
                if(x >= _settings.Width)
                {
                    x = 0;
                    y++;
                }                
            }
        }

        public GameObject GetVisual(Tile tile)
        {
            return _tilesVisual[tile.Coords.x, tile.Coords.y];
        }

        public GameObject GetVisual(Vector3 worldPosition)
        {
            if (Grid.TryGetTile( worldPosition, out Tile t))
            {
                return _tilesVisual[t.Coords.x, t.Coords.y];
            }
            else
            {
                return null;
            }
            
        }


    }
}

