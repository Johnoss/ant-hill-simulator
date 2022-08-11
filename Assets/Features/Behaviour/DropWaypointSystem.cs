using Features.Game;
using Features.Position;
using Features.Spawning;
using Features.Unit;
using Features.Waypoints;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Features.Behaviour
{
    public class DropWaypointSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<UnitComponent, PoseComponent>> _units;

        private readonly EcsPoolInject<SpawnWaypointEvent> _spawnUnitEventPool = Idents.Worlds.Events;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _units.Value)
            {
                //TODO add timer
                if (Random.Range(0, 100 * Time.deltaTime) > .1f * Time.deltaTime)
                {
                    continue;
                }
                
                var positionComponent = _units.Pools.Inc2.Get(entity);

                var eventEntity = _spawnUnitEventPool.Value.GetWorld().NewEntity();
                ref var eventComponent = ref _spawnUnitEventPool.Value.Add(eventEntity);
                eventComponent.Position = positionComponent.Pose.position;
                eventComponent.Type = (WaypointType)Random.Range(0, 2);
            }
        }
    }
}