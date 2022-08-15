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

        public Vector2Int GridResolution => _gridResolution;
        public float CellWidth => _cellWidth;
    }
}