using Features.Position;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Features.Movement
{
    public class PoseSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PoseComponent>> _positionPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _positionPool.Value)
            {
                var positionComponent = _positionPool.Pools.Inc1.Get(entity);

                positionComponent.Transform.localPosition = positionComponent.Pose.position;
                positionComponent.Transform.rotation = positionComponent.Pose.rotation;
            }
        }
    }
}