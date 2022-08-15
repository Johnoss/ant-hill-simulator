using System.Linq;
using Features.Waypoints;
using UnityEngine;

namespace Features.Grid
{
    public class GridService
    {
        private readonly NodeWeights[,] _gridNodeWeights;

        private readonly int _gridX;
        private readonly int _gridY;
        private readonly float _cellWidth;

#if UNITY_EDITOR
        private int PopulatedCellsCount
        {
            get
            {
                return _gridNodeWeights.Cast<NodeWeights>().Count(nodeWeights =>
                    nodeWeights.GoalMarkerWeight != 0 || nodeWeights.HomeMarkerWeight != 0);
            }
        }
#endif

        public GridService(GridConfig gridConfig)
        {
            _gridX = gridConfig.GridResolution.x;
            _gridY = gridConfig.GridResolution.y;
            _cellWidth = gridConfig.CellWidth;
            
            _gridNodeWeights = new NodeWeights[_gridX, _gridY];
        }

        public void UpdateWeights(Vector3 position, WaypointComponent waypointComponent, bool isRemoving = false)
        {
            var gridCoordinates = GetCoordinatesFromPosition(position);

            var direction = isRemoving ? -1 : 1;
            
            ref var nodeWeights = ref _gridNodeWeights[gridCoordinates.Item1, gridCoordinates.Item2];
            nodeWeights.HomeMarkerWeight += direction * waypointComponent.HomeWaypointWeight;
            nodeWeights.GoalMarkerWeight += direction * waypointComponent.GoalWaypointWeight;
#if UNITY_EDITOR
            //Debug.Log($"Populated Cells Count: {PopulatedCellsCount}");
#endif
        }

        public NodeWeights GetWeightsForPosition(Vector2 position)
        {
            var gridCoordinates = GetCoordinatesFromPosition(position);

            return _gridNodeWeights[gridCoordinates.Item1, gridCoordinates.Item2];
        }

        private (int, int) GetCoordinatesFromPosition(Vector3 position)
        {
            var cellRow = (int)(position.x / _cellWidth);
            var cellColumn = (int)(position.z / _cellWidth);
            return (cellRow, cellColumn);
        }
    }
}