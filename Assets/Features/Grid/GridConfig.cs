using UnityEngine;

namespace Features.Grid
{
    [CreateAssetMenu(menuName = "Create GridConfig", fileName = "GridConfig", order = 0)]
    public class GridConfig : ScriptableObject
    {
        [SerializeField]
        private Vector2Int _gridResolution;
        [SerializeField]
        private float _cellWidth;
        [SerializeField]
        private Color _gridColor;
        [SerializeField]
        private bool _drawGrid;

        public Vector2Int GridResolution => _gridResolution;
        public float CellWidth => _cellWidth;
        public Color GridColor => _gridColor;
        public bool DrawGrid => _drawGrid;
    }
}