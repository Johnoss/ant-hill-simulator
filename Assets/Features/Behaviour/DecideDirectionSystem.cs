using System.Collections.Generic;
using System.Linq;
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
        private readonly EcsFilterInject<Inc<DecideDirectionCommand, PoseComponent, VisionComponent>> _decideCommands;
                
        private readonly EcsFilterInject<Inc<WaypointComponent, PoseComponent>> _waypointPool;
        
        private readonly EcsPoolInject<VisionComponent> _visionPool;
        private readonly EcsPoolInject<PoseComponent> _posePool;

        private readonly EcsCustomInject<UnitConfig> _unitConfig;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _decideCommands.Value)
            {
                var unitPosition = _posePool.Value.Get(entity);
                var visionComponent = _visionPool.Value.Get(entity);

                var zones = new float[_unitConfig.Value.AntVisionZones];
                
                foreach (var waypointEntity in _waypointPool.Value)
                {
                    var waypointPose = _waypointPool.Pools.Inc2.Get(waypointEntity);
                    var zone = visionComponent.IsInZone(unitPosition, waypointPose);
                    if (zone >= 0)
                    {
                        zones[zone] += _waypointPool.Pools.Inc1.Get(waypointEntity).WaypointWeight;
                    }
                }

                if (zones.Any(zone => zone != 0))
                {
                     Debug.Log($"zones weight: {string.Join(',', zones.ToList())}");
                }
            }
        }
    }
}