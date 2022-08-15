using Features.Behaviour;
using Features.Game;
using Features.Grid;
using Features.Movement;
using Features.Position;
using Features.Timer;
using Features.Utils;
using Features.Waypoints;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Features.Unit
{
    public class SpawnUnitSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<SpawnUnitEvent>> _spawnEvents = Idents.Worlds.Events;

        private readonly EcsPoolInject<UnitComponent> _unitPool;
        private readonly EcsPoolInject<PoseComponent> _posePool;
        private readonly EcsPoolInject<TransformComponent> _transformPool;
        private readonly EcsPoolInject<RotateComponent> _rotatePool;
        private readonly EcsPoolInject<MoveCommand> _moveCommandPool;
        private readonly EcsPoolInject<VisionComponent> _visionPool;

        private readonly EcsCustomInject<UnitConfig> _unitConfig;
        private readonly EcsCustomInject<GridConfig> _gridConfig;

        private readonly EcsPoolInject<TimerComponent> _timerPool = Idents.Worlds.Timer;
        private readonly EcsPoolInject<DropWaypointTimerComponent> _dropWaypointPool = Idents.Worlds.Timer;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _spawnEvents.Value)
            {
                var world = _unitPool.Value.GetWorld();
                var unitEntity = world.NewEntity();

                ref var poseComponent = ref _posePool.Value.Add(unitEntity);
                ref var unitComponent = ref _unitPool.Value.Add(unitEntity);
                ref var rotateComponent = ref _rotatePool.Value.Add(unitEntity);
                ref var transformComponent = ref _transformPool.Value.Add(unitEntity);
                ref var visionComponent = ref _visionPool.Value.Add(unitEntity);
                _moveCommandPool.Value.Add(unitEntity);

                //TODO Add spawning point on map - this is temporary center
                poseComponent.Pose.position =
                    new Vector3(_gridConfig.Value.GridResolution.x, 0, _gridConfig.Value.GridResolution.y) *
                    _gridConfig.Value.CellWidth / 2;
                
                var unitView = Object.Instantiate(_unitConfig.Value.AntPrefab);

                var unitTransform = unitView.transform;
                unitTransform.localEulerAngles = Random.Range(0, 360) * Vector3.up;
                
                unitComponent.MoveSpeed = _unitConfig.Value.AntSpeed;

                rotateComponent.RotateSpeed = _unitConfig.Value.AntTurnSpeed;
                //TODO make extension
                rotateComponent.TargetRotation = Quaternion.Euler(0, Random.Range(0,360), 0);
                
                transformComponent.Transform = unitTransform;

                visionComponent.VisionRadius = _unitConfig.Value.AntVisionRadius;
                visionComponent.AngularDeviation = _unitConfig.Value.AntAngularDeviation;
                visionComponent.ZonesCount = _unitConfig.Value.AntVisionZones;

                CreateDropWaypointTimer(unitEntity);
            }
        }

        private void CreateDropWaypointTimer(int unitEntity)
        {
            var waypointTimerEntity = _timerPool.Value.GetWorld().NewEntity();
            
            ref var timerComponent = ref _timerPool.Value.Add(waypointTimerEntity);
            ref var dropWaypointComponent = ref _dropWaypointPool.Value.Add(waypointTimerEntity);

            timerComponent.DefaultSeconds = _unitConfig.Value.GetDropIntervalSeconds();
            timerComponent.RemainingTimerSeconds = timerComponent.DefaultSeconds;
            
            dropWaypointComponent.DropperEntity = unitEntity;
        }
    }
}