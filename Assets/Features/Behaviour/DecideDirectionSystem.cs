using System.Collections.Generic;
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
                GetWaypointZones(visionComponent, unitPoseComponent, zones);
                waypointsWatch.Stop();
                
                waypointsTicks += waypointsWatch.ElapsedTicks;

                var newDirectionZone = zones.GetWeighedRandomIndex(_waypointConfig.Value.BaseWaypointWeight);
                var newDirectionAngle = visionComponent.GetZoneDirectionAngle(newDirectionZone)
                    .AddRandomRange(_unitConfig.Value.AntNewDirectionRandomDeviation);

                rotateComponent.TargetRotation =
                    Quaternion.AngleAxis(unitPoseComponent.Pose.rotation.eulerAngles.y + newDirectionAngle, Vector3.up);
            }
            
            watch.Stop();
            //Debug.Log($"DecideDirectionSystem took: {watch.ElapsedTicks} ticks - out of which {waypointsTicks}:");
        }

        private void GetWaypointZones(VisionComponent visionComponent, PoseComponent unitPoseComponent, IList<float> zones)
        {
            var radius = visionComponent.VisionRadius;
            var origin = unitPoseComponent.Pose.position.ToXZPlane();

            foreach (var entity in _waypointPool.Value)
            {
                var waypointPose = _waypointPool.Pools.Inc2.Get(entity);
                var zone = visionComponent.IsInZone(unitPoseComponent, waypointPose);
                if (zone >= 0)
                {
                    zones[zone] += _waypointPool.Pools.Inc1.Get(entity).WaypointWeight;
                }
            }
        }
    }
}