using Features.Game;
using Features.Position;
using Features.Timer;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Features.Waypoints
{
    //TODO question - two system handling one piece of logic
    //this one: Catches timer expired commands and pushes spawn event
    // SpawnWaypointSystem: catches events and actually spawns the waypoint
    // Should this be unified?
    public class DropWaypointTimerSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TimerExpiredCommand, TimerComponent, DropWaypointTimerComponent>> _timerExpiredPool =
            Idents.Worlds.Timer;

        private readonly EcsPoolInject<SpawnWaypointEvent> _spawnWaypointEventPool = Idents.Worlds.Events;
        private readonly EcsPoolInject<PoseComponent> _poseEventPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var timerEntity in _timerExpiredPool.Value)
            {
                var dropComponent = _timerExpiredPool.Pools.Inc3.Get(timerEntity);

                var dropperPoseComponent = _poseEventPool.Value.Get(dropComponent.DropperEntity);
                
                var eventEntity = _spawnWaypointEventPool.Value.GetWorld().NewEntity();
                ref var eventComponent = ref _spawnWaypointEventPool.Value.Add(eventEntity);
                eventComponent.Position = dropperPoseComponent.Pose.position;
                //TODO placeholder! Decide according to current state (carrying food / not carrying food)
                eventComponent.WaypointWeight = -1;

                ref var timerComponent = ref _timerExpiredPool.Pools.Inc2.Get(timerEntity);
                timerComponent.RemainingTimerSeconds = timerComponent.DefaultSeconds;
            }
        }
    }

    public struct DropWaypointTimerComponent
    {
        public int DropperEntity;
    }
}