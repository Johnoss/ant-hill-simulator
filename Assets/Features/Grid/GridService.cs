using System.Linq;
using Features.Waypoints;
using UnityEngine;

namespace Features.Grid
{
    public class GridService
    {
        private readonly GridConfig _gridConfig;
        private NodeWeights[,] _gridNodeWeights;

        private readonly int _gridX;
        private readonly int _gridY;
        public float CellWidth { get; }
        public Vector2Int GridCenter { get; }

#if UNITY_EDITOR
        private int PopulatedCellsCount
        {
            get
            {
                return _gridNodeWeights.Cast<NodeWeights>().Count(nodeWeights =>
                    nodeWeights.Weights.Sum() != 0);
            }
        }
#endif

        public GridService(GridConfig gridConfig)
        {
            _gridConfig = gridConfig;
            _gridX = gridConfig.GridResolution.x;
            _gridY = gridConfig.GridResolution.y;
            CellWidth = gridConfig.CellWidth;
            
            PopulateGrid();

            GridCenter = gridConfig.GridResolution / 2;
        }

        private void PopulateGrid()
        {
            _gridNodeWeights = new NodeWeights[_gridX, _gridY];
            
            for (var row = 0; row < _gridNodeWeights.GetLength(0); row++)
            {
                if (_gridConfig.DrawGrid)
                {
                    Debug.DrawLine(new Vector3(row * CellWidth, 0, 0), new Vector3(row * CellWidth, 0, _gridX), _gridConfig.GridColor, float.MaxValue);
                }
                for (var column = 0; column < _gridNodeWeights.GetLength(1); column++)
                {
                    _gridNodeWeights[row, column] = new NodeWeights();
                    
                    if (_gridConfig.DrawGrid)
                    {
                        Debug.DrawLine(new Vector3(0, 0, column * CellWidth), new Vector3(_gridY, 0, column * CellWidth), _gridConfig.GridColor, float.MaxValue);
                    }
                }
            }
        }

        public void UpdateWeights(Vector3 position, WaypointComponent waypointComponent, bool isRemoving = false)
        {
            var nodeWeights = GetWeightsForPosition(position);

            var direction = isRemoving ? -1 : 1;
            
            nodeWeights.Weights[0] += direction * waypointComponent.HomeWaypointWeight;
            nodeWeights.Weights[1] += direction * waypointComponent.GoalWaypointWeight;
#if UNITY_EDITOR
            //Debug.Log($"Populated Cells Count: {PopulatedCellsCount}");
#endif
        }

        public NodeWeights GetWeightsForPosition(Vector3 position)
        {
            var gridCoordinates = GetCoordinatesFromPosition(position);

            var weightsForPosition = _gridNodeWeights[gridCoordinates.x, gridCoordinates.y];

            return weightsForPosition;
        }

        public bool IsWithinBounds(Vector2 position)
        {
            var coordinates = GetCoordinatesFromPosition(position);
            return 0 <= coordinates.x && coordinates.x < _gridX && 0 <= coordinates.y && coordinates.y < _gridY;
        }
        
        public Vector2Int GetCoordinatesFromPosition(Vector3 position)
        {
            var cellRow = (int)(position.x * (1 / CellWidth));
            var cellColumn = (int)(position.z * (1 / CellWidth));
            return new Vector2Int(cellRow, cellColumn);
        }
    }
}