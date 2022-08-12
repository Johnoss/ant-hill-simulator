using System;
using Features.Game;
using Features.Position;
using Features.Waypoints;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Features.Spawning
{
    public class SpawnWaypointSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<SpawnWaypointEvent>> _spawnWaypointEvents = Idents.Worlds.Events;

        private readonly EcsPoolInject<WaypointComponent> _waypointPool;
        private readonly EcsPoolInject<PoseComponent> _positionPool;

        private readonly EcsCustomInject<WaypointConfig> _waypointConfig;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _spawnWaypointEvents.Value)
            {
                var world = _positionPool.Value.GetWorld();
                var waypointEntity = world.NewEntity();
                ref var waypointComponent = ref _waypointPool.Value.Add(waypointEntity);
                

                var spawnWaypointEvent = _spawnWaypointEvents.Pools.Inc1.Get(entity);
                var waypointMaterial = _waypointConfig.Value.GetWaypointMaterial(spawnWaypointEvent.WaypointWeight);
                
                var waypointView = Object.Instantiate(_waypointConfig.Value.WaypointView);
                waypointView.Setup(waypointMaterial);

                waypointComponent.WaypointWeight = spawnWaypointEvent.WaypointWeight;
                
                ref var positionComponent = ref _positionPool.Value.Add(waypointEntity);
                positionComponent.Pose = new Pose(spawnWaypointEvent.Position, Quaternion.identity);
                positionComponent.Transform = waypointView.transform;
            }
        }
    }
}