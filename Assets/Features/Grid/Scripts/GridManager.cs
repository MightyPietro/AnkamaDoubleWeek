using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WeekAnkama
{
    public class GridManager : MonoBehaviour
    {
        private static Grid _grid;

        [SerializeField] private GridLevel _settings;
        private GameObject[,] _tilesVisual;                

        public static Grid Grid => _grid;

        private void Awake()
        {
            _grid = new Grid(_settings.Width, _settings.Heigth, _settings.CellSize, 
                (grid, coords) => { return new Tile(grid, coords, grid.GetTileWorldPosition(coords.x, coords.y)); }
                , _settings.NormalizedOffset);

            _tilesVisual = new GameObject[_grid.Width, _grid.Heigth];

            for (int y = 0; y < _grid.Heigth; y++)
            {
                for (int x = 0; x < _grid.Width; x++)
                {
                    //_tilesVisual[x, y] = Instantiate(,this);
                }
            }
        }


    }
}

