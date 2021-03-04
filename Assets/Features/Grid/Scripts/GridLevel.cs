using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int Width => _width;
    public int Heigth => _heigth;
    public Vector2Int CellSize => _cellSize;
    public Vector2 NormalizedOffset => _normalizedOffset;
}
