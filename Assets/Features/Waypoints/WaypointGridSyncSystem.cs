using Features.Game;
using Features.Grid;
using Features.Lifespan;
using Features.Position;
using Features.Timer;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Features.Waypoints
{
    public class WaypointGridSyncSystem : IEcsRunSystem
    {
        
        private readonly EcsFilterInject<Inc<TimerExpiredCommand, LifespanTimerComponent>> _lifespanTimerExpiredPool =
            Idents.Worlds.Timer;

        private EcsPoolInject<WaypointComponent> _waypointPool;
        private EcsPoolInject<PoseComponent> _posePool;
        private EcsCustomInject<GridService> _gridService;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _lifespanTimerExpiredPool.Value)
            {
                var lifespanTimerComponent = _lifespanTimerExpiredPool.Pools.Inc2.Get(entity);
                var lifespanEntity = lifespanTimerComponent.LifespanEntity;
                
                if (_waypointPool.Value.Has(lifespanEntity) && _posePool.Value.Has(lifespanEntity))
                {
                    var waypointComponent = _waypointPool.Value.Get(lifespanEntity);
                    var poseComponent = _posePool.Value.Get(lifespanEntity);
                    
                    _gridService.Value.UpdateWeights(poseComponent.Pose.position, waypointComponent, true);
                }
            }
        }
    }
}