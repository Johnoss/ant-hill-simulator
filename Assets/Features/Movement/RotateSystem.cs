using Features.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Features.Movement
{
    public class RotateSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<RotateComponent>> _unitPool;
        private const double DiffToStop = 3;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitPool.Value)
            {
                ref var rotateComponent = ref _unitPool.Pools.Inc1.Get(entity);

                var currentRotation = rotateComponent.Transform.rotation;

                rotateComponent.Transform.rotation = Quaternion.Lerp(currentRotation,
                    rotateComponent.TargetRotation, rotateComponent.RotateSpeed * Time.deltaTime);

                if ((currentRotation.eulerAngles - rotateComponent.TargetRotation.eulerAngles).sqrMagnitude <=
                    DiffToStop)
                {
                    rotateComponent.TargetRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                }
            }
        }
    }
}