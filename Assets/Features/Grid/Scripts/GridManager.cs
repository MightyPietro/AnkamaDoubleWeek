using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WeekAnkama
{
    public class GridManager : MonoBehaviour
    {
        private static Grid _grid;
        private static GameObject[,] _tilesVisual;   

        [SerializeField] private GridLevel _settings;
        [SerializeField] private GameObject[] _floor;

        [SerializeField] private GameObject _highlightTile;

        public static Grid Grid => _grid;
        public static GameObject[,] TilesVisual => _tilesVisual;

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

        private void Start()
        {

            int currentIdx = 0;
            foreach (var item in _settings.CustomCellPosition)
            {
                if (_grid.TryGetTile(item, out Tile t))
                {
                    t.SetTileEffect(_settings.CorrespondingEffect[currentIdx]);
                }
                currentIdx++;
            }
        }

        public static GameObject GetVisual(Tile tile)
        {
            return _tilesVisual[tile.Coords.x, tile.Coords.y];
        }

        public static GameObject GetVisual(Vector3 worldPosition)
        {
            if (Grid.TryGetTile(worldPosition, out Tile t))
            {
                return _tilesVisual[t.Coords.x, t.Coords.y];
            }
            else
            {
                return null;
            }            
        }

        public static void ChangeColor(Tile tile, Color color)
        {
            GameObject obj = GridManager.GetVisual(tile);
            int colorInt = 0;
            if(color == Color.cyan)
            {
                obj.GetComponent<TileFeedback>().Show(1);
            }
            else if(color == Color.green)
            {
                obj.GetComponent<TileFeedback>().Show(2);
            }

        }

        public static void Reset(Tile tile, Color color)
        {
            GameObject obj = GridManager.GetVisual(tile);
            obj.GetComponent<TileFeedback>().Hide();
        }


    }
}

