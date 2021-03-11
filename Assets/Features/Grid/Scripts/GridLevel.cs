using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "GridLevel", menuName = "Assets/GridSettings")]
    public class GridLevel : ScriptableObject
    {
        [Range(0, 50)]
        [SerializeField] private int _width = 7;
        [Range(0, 50)]
        [SerializeField] private int _heigth = 7;

        [Space]
        [SerializeField] private Vector2Int _cellSize = Vector2Int.one;

        [SerializeField] private Vector2 _normalizedOffset = new Vector2(-0.5f, -0.5f);

        [Space]
        [SerializeField] private List<Vector2Int> _customCellPosition;
        [SerializeField] private List<TileEffect> _correspondingEffect;

        public int Width => _width;
        public int Heigth => _heigth;
        public Vector2Int CellSize => _cellSize;
        public Vector2 NormalizedOffset => _normalizedOffset;

        public List<Vector2Int> CustomCellPosition => _customCellPosition; 
        public List<TileEffect> CorrespondingEffect => _correspondingEffect;
    }
}

