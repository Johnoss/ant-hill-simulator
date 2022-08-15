using Features.Behaviour;
using Features.Game;
using Features.Position;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Features.Movement
{
    public class RotateSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<RotateComponent, PoseComponent>, Exc<StaticPoseComponent>> _unitPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitPool.Value)
            {
                ref var rotateComponent = ref _unitPool.Pools.Inc1.Get(entity);
                ref var poseComponent = ref _unitPool.Pools.Inc2.Get(entity);

                var currentRotation = poseComponent.Pose.rotation;

                poseComponent.Pose.rotation = Quaternion. Lerp(currentRotation,
                    rotateComponent.TargetRotation, rotateComponent.RotateSpeed * Time.deltaTime);
                
                
#if DEBUG && UNITY_EDITOR
                // Debug.DrawLine(poseComponent.Pose.position, poseComponent.Pose.position + poseComponent.Pose.forward, Color.black);
                // var targetPose = new Pose(poseComponent.Pose.position, rotateComponent.TargetRotation);
                // Debug.DrawLine(poseComponent.Pose.position, poseComponent.Pose.position + targetPose.forward, Color.grey);
#endif
            }
        }
    }
}