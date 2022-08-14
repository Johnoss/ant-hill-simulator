using Features.Game;
using Features.Lifespan;
using Features.Position;
using Features.Timer;
using Features.ViewPool;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Features.Waypoints
{
    public class SpawnWaypointSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<SpawnWaypointEvent>> _spawnWaypointEvents = Idents.Worlds.Events;
        private readonly EcsPoolInject<LifespanTimerComponent> _lifespanPool = Idents.Worlds.Timer;

        private readonly EcsPoolInject<WaypointComponent> _waypointPool;
        private readonly EcsPoolInject<PoseComponent> _positionPool;
        private readonly EcsPoolInject<TransformComponent> _transformPool;

        private readonly EcsPoolInject<TimerComponent> _timerPool = Idents.Worlds.Timer;

        private readonly EcsCustomInject<WaypointConfig> _waypointConfig;

        private readonly EcsCustomInject<ViewPool<WaypointView>> _viewPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _spawnWaypointEvents.Value)
            {
                var spawnWaypointEvent = _spawnWaypointEvents.Pools.Inc1.Get(entity);
                
                var world = _positionPool.Value.GetWorld();
                var waypointEntity = world.NewEntity();
                
                ref var poseComponent = ref _positionPool.Value.Add(waypointEntity);
                ref var waypointComponent = ref _waypointPool.Value.Add(waypointEntity);

                CreateLifespanTimer(waypointEntity, world);
                
                poseComponent.Pose = new Pose(spawnWaypointEvent.Position, Quaternion.identity);
                
                waypointComponent.WaypointWeight = spawnWaypointEvent.WaypointWeight;
                
                if (_waypointConfig.Value.CreateWaypointsGameObjects)
                {
                    var waypointMaterial = _waypointConfig.Value.GetWaypointMaterial(spawnWaypointEvent.WaypointWeight);
                    var waypointView = _viewPool.Value.GetOrCreate();
                    waypointView.Setup(waypointMaterial);
                    
                    ref var transformComponent = ref _transformPool.Value.Add(waypointEntity);
                    transformComponent.Transform = waypointView.transform;
                }
            }
        }

        private void CreateLifespanTimer(int waypointComponent, EcsWorld world)
        {
            var lifespanTimerEntity = _timerPool.Value.GetWorld().NewEntity();
            ref var timerComponent = ref _timerPool.Value.Add(lifespanTimerEntity);
            ref var lifespanTimerComponent = ref _lifespanPool.Value.Add(lifespanTimerEntity);

            timerComponent.RemainingTimerSeconds = _waypointConfig.Value.BaseLifespanSeconds;
            lifespanTimerComponent.LifespanEntity = waypointComponent;
            lifespanTimerComponent.World = world;
        }
    }
}