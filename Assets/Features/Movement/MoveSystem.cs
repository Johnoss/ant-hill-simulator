using Features.Position;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Features.Movement
{
    public class MoveSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<MoveComponent, PositionComponent>> _movePool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _movePool.Value)
            {
                var moveComponent = _movePool.Pools.Inc1.Get(entity);
                var positionComponent = _movePool.Pools.Inc2.Get(entity);

                positionComponent.Transform.localPosition +=
                    positionComponent.Transform.forward * moveComponent.MoveSpeed * Time.deltaTime;
            }
        }
    }
}