using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Features.Position
{
    public class PoseSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PoseComponent, TransformComponent>> _positionPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _positionPool.Value)
            {
                var positionComponent = _positionPool.Pools.Inc1.Get(entity);
                var transformComponent = _positionPool.Pools.Inc2.Get(entity);

                transformComponent.Transform.localPosition = positionComponent.Pose.position;
                transformComponent.Transform.rotation = positionComponent.Pose.rotation;
            }
        }
    }
}