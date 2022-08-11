using Features.Behaviour;
using Features.Position;
using UnityEngine;

namespace Features.Utils
{
    public static class VisionComponentExtensions
    {
        public static bool IsInRange(this VisionComponent visionComponent, Vector3 visionOrigin,
            PoseComponent comparePoseComponent)
        {
            var radiusSquared = visionComponent.VisionRadius * visionComponent.VisionRadius;
            
            var differenceSquared = (comparePoseComponent.Pose.position - visionOrigin).sqrMagnitude;
            return radiusSquared > differenceSquared;
        }
    }
}