using Features.Position;
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
                
        private readonly EcsFilterInject<Inc<WaypointComponent, PoseComponent>> _repellentPool;
        
        private readonly EcsPoolInject<VisionComponent> _visionPool;
        private readonly EcsPoolInject<PoseComponent> _posePool;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _decideCommands.Value)
            {
                var unitPosition = _posePool.Value.Get(entity).Pose.position;
                var visionComponent = _visionPool.Value.Get(entity);

                var repellentCount = 0;
                
                foreach (var repellentEntity in _repellentPool.Value)
                {
                    var repellentPose = _repellentPool.Pools.Inc2.Get(repellentEntity);
                    if (visionComponent.IsInRange(unitPosition, repellentPose))
                    {
                        repellentCount++;
                    }
                }
            }
        }
    }
}