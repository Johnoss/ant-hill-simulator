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
        private readonly EcsFilterInject<Inc<RotateComponent, PoseComponent>> _unitPool;

        private readonly EcsPoolInject<UnitReachedTargetEvent> _targetReachedEvents = Idents.Worlds.Events;

        private const double DiffToStop = 5;

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
                    //TODO publish event about turn completed
                    
                    ref var reachedEvent =
                        ref _targetReachedEvents.Value.Add(_targetReachedEvents.Value.GetWorld().NewEntity());
                
                    reachedEvent.UnitEntity = entity;
                }
            }
        }
    }
}