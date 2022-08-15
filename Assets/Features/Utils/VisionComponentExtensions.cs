using Features.Behaviour;
using Features.Grid;
using Features.Position;
using UnityEngine;

namespace Features.Utils
{
    public static class VisionComponentExtensions
    {
        public static float[] GetZonesWeights(this VisionComponent vision, PoseComponent pose, GridService gridService,
            int[] seekPattern)
        {
            var zoneNodeWeights = new float[3];
            
            for (var i = 0; i < 3; i++)
            {
                var direction = vision.GetZoneDirection(pose, i);
                zoneNodeWeights[i] = vision.GetZoneWeights(pose, gridService, direction, seekPattern);
            }

            return zoneNodeWeights;
        }

        private static float GetZoneWeights(this VisionComponent visionComponent, PoseComponent pose,
            GridService gridService, Vector3 normalizedDirection, int[] seekPattern)
        {
            var totalWeight = 0f;
            for (var i = 0; i < visionComponent.VisionRadius; i++)
            {
                var direction = normalizedDirection * gridService.CellWidth * i;
                var targetPosition = pose.Pose.position + direction;
#if UNITY_EDITOR && DEBUG
                var coordinates = gridService.GetCoordinatesFromPosition(targetPosition);
                var cellCentreOffset = new Vector3(gridService.CellWidth / 2f, 0, gridService.CellWidth / 2f);
                var cellCoordinates =
                    new Vector3(coordinates.x * gridService.CellWidth, 0, coordinates.y * gridService.CellWidth) +
                    cellCentreOffset;
                var color = i == 0 ? Color.blue : Color.magenta;
//                Debug.DrawLine(pose.Pose.position, cellCoordinates, color, 1f, false);
#endif
                totalWeight += gridService.GetWeightsForPosition(targetPosition).GetWeightFromNode(seekPattern);
            }

            return totalWeight;
        }

        public static Vector3 GetZoneDirection(this VisionComponent vision, PoseComponent pose, int zoneIndex)
        {
            return Quaternion.Euler(0, vision.SideVisionAngle * zoneIndex - 1, 0) * pose.Pose.forward;
        }
    }
}