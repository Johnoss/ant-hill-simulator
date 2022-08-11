using Features.Position;
using Features.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Features.Movement
{
    public class RotateSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<RotateComponent, PoseComponent>> _unitPool;
        private const double DiffToStop = 3;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitPool.Value)
            {
                ref var rotateComponent = ref _unitPool.Pools.Inc1.Get(entity);
                ref var poseComponent = ref _unitPool.Pools.Inc2.Get(entity);

                var currentRotation = poseComponent.Pose.rotation;

                poseComponent.Pose.rotation = Quaternion.Lerp(currentRotation,
                    rotateComponent.TargetRotation, rotateComponent.RotateSpeed * Time.deltaTime);

                if ((currentRotation.eulerAngles - rotateComponent.TargetRotation.eulerAngles).sqrMagnitude <=
                    DiffToStop)
                {
                    //TODO make new decision
                    rotateComponent.TargetRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                }
            }
        }
    }
}