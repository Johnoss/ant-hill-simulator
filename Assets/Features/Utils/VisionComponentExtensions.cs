using System.IO;
using Features.Behaviour;
using Features.Position;
using UnityEngine;

namespace Features.Utils
{
    public static class VisionComponentExtensions
    {
        public static int IsInZone(this VisionComponent visionComponent, PoseComponent visionOrigin,
            PoseComponent comparePoseComponent)
        {
            var radiusSquared = visionComponent.VisionRadius * visionComponent.VisionRadius;

            var direction = comparePoseComponent.Pose.position - visionOrigin.Pose.position;
            var isInRange = radiusSquared > direction.sqrMagnitude;

            if (!isInRange)
            {
                return -1;
            }

            var poseForward = visionOrigin.Pose.forward;
            var angle = Vector3.Angle(direction, poseForward) * GetAngleDirectionMultiplier(poseForward, direction);
            var angularDeviation = visionComponent.AngularDeviation;

            if (!angle.IsWithinRange(-angularDeviation, angularDeviation))
            {
                return -1;
            }

            var zoneAngularWidth = visionComponent.GetZoneAngularWidth();

            for (var zone = 0; zone < visionComponent.ZonesCount; zone++)
            {
                if (angle <= -angularDeviation + zoneAngularWidth * (zone + 1))
                {
                    return zone;
                }
            }

            throw new InvalidDataException();
        }

        private static float GetZoneAngularWidth(this VisionComponent visionComponent)
        {
            return visionComponent.AngularDeviation * 2f / visionComponent.ZonesCount;
        }

        public static float GetZoneDirectionAngle(this VisionComponent visionComponent, int zoneIndex)
        {
            var angularWidth = visionComponent.GetZoneAngularWidth();
            return -visionComponent.AngularDeviation + (angularWidth / 2f + zoneIndex * angularWidth);
        }

        private static float GetAngleDirectionMultiplier(Vector3 forward, Vector3 targetDirection)
        {
            var cross = Vector3.Cross(forward, targetDirection);
            var dot = Vector3.Dot(cross, Vector3.up);

            return dot switch
            {
                > 0f => 1f,
                < 0f => -1f,
                _ => 0f
            };
        }
    }
}