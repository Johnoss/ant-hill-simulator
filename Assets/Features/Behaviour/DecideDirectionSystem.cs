using System.Collections.Generic;
using Features.Grid;
using Features.Movement;
using Features.Position;
using Features.Unit;
using Features.Utils;
using Features.Waypoints;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Features.Behaviour
{
    public class DecideDirectionSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DecideDirectionCommand, PoseComponent, VisionComponent, RotateComponent>>
            _decideCommandPool;
                
        private readonly EcsFilterInject<Inc<WaypointComponent, PoseComponent>> _waypointPool;
        
        private readonly EcsCustomInject<UnitConfig> _unitConfig;
        private readonly EcsCustomInject<WaypointConfig> _waypointConfig;
        private EcsCustomInject<GridService> _gridService;

        public void Run(IEcsSystems systems)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var waypointsTicks = 0L;
            foreach (var entity in _decideCommandPool.Value)
            {
                var unitPoseComponent = _decideCommandPool.Pools.Inc2.Get(entity);
                var visionComponent = _decideCommandPool.Pools.Inc3.Get(entity);
                ref var rotateComponent = ref _decideCommandPool.Pools.Inc4.Get(entity);

                var zones = new float[_unitConfig.Value.AntVisionZones];
                
                var waypointsWatch = System.Diagnostics.Stopwatch.StartNew();
                waypointsWatch.Stop();
                
                waypointsTicks += waypointsWatch.ElapsedTicks;

                //TODO define seek patterns
                // -1: avoid home
                // 1: follow food
                var seekFoodPattern = new[]{-1, 1};
                
                var zonesWeights = visionComponent.GetZonesWeights(unitPoseComponent, _gridService.Value, seekFoodPattern);
                
                var decidedZone = zonesWeights.GetWeighedRandomIndex(_waypointConfig.Value.BaseWaypointWeight);
                //Debug.Log($"Weights: {string.Join(',', zonesWeights)}, decided: {decidedZone}");

                rotateComponent.TargetRotation =
                    Quaternion.LookRotation(visionComponent.GetZoneDirection(unitPoseComponent, decidedZone));
            }
            
            watch.Stop();
            //Debug.Log($"DecideDirectionSystem took: {watch.ElapsedTicks} ticks - out of which {waypointsTicks}:");
        }
    }
}