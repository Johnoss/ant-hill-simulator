using Features.Behaviour;
using Features.Game;
using Features.Movement;
using Features.Position;
using Features.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Features.Spawning
{
    public class SpawnUnitSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<SpawnUnitEvent>> _spawnEvents = Idents.Worlds.Events;

        private readonly EcsPoolInject<UnitComponent> _unitPool;
        private readonly EcsPoolInject<PoseComponent> _positionPool;
        private readonly EcsPoolInject<RotateComponent> _rotatePool;
        private readonly EcsPoolInject<MoveCommand> _moveCommandPool;
        private readonly EcsPoolInject<VisionComponent> _visionPool;

        private readonly EcsCustomInject<UnitConfig> _unitConfig;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _spawnEvents.Value)
            {
                var world = _unitPool.Value.GetWorld();
                var unitEntity = world.NewEntity();

                ref var unitComponent = ref _unitPool.Value.Add(unitEntity);
                ref var rotateComponent = ref _rotatePool.Value.Add(unitEntity);
                ref var positionComponent = ref _positionPool.Value.Add(unitEntity);
                ref var visionComponent = ref _visionPool.Value.Add(unitEntity);
                _moveCommandPool.Value.Add(unitEntity);
                
                var unitView = Object.Instantiate(_unitConfig.Value.AntPrefab);

                var unitTransform = unitView.transform;
                unitTransform.localEulerAngles = Random.Range(0, 360) * Vector3.up;
                
                unitComponent.MoveSpeed = _unitConfig.Value.AntSpeed;

                rotateComponent.RotateSpeed = _unitConfig.Value.AntTurnSpeed;
                //TODO make extension
                rotateComponent.TargetRotation = Quaternion.Euler(0, Random.Range(0,360), 0);
                
                positionComponent.Transform = unitTransform;

                visionComponent.VisionRadius = _unitConfig.Value.AntVisionRadius;
            }
        }
    }
}