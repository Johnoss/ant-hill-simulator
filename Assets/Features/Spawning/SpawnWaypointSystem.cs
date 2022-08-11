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

        private readonly EcsPoolInject<RepellerComponent> _repellerPool;
        private readonly EcsPoolInject<AttractorComponent> _attractorPool;
        private readonly EcsPoolInject<PoseComponent> _positionPool;

        private readonly EcsCustomInject<WaypointConfig> _waypointConfig;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _spawnWaypointEvents.Value)
            {
                var world = _positionPool.Value.GetWorld();
                var waypointEntity = world.NewEntity();

                var spawnWaypointEvent = _spawnWaypointEvents.Pools.Inc1.Get(entity);
                var waypointSetting = _waypointConfig.Value.GetWaypointViewSetting(spawnWaypointEvent.Type);
                
                var waypointView = Object.Instantiate(_waypointConfig.Value.WaypointView);
                waypointView.Setup(waypointSetting);

                switch (spawnWaypointEvent.Type)
                {
                    case WaypointType.Attract:
                        _attractorPool.Value.Add(waypointEntity);
                        break;
                    case WaypointType.Repel:
                        _repellerPool.Value.Add(waypointEntity);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                ref var positionComponent = ref _positionPool.Value.Add(waypointEntity);
                positionComponent.Pose = new Pose(spawnWaypointEvent.Position, Quaternion.identity);
                positionComponent.Transform = waypointView.transform;
            }
        }
    }
}