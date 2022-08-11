using Features.Position;
using Features.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Features.Movement
{
    public class StartMovingSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<UnitComponent, PositionComponent, MoveCommand>, Exc<MoveComponent>>
            _unitPool;

        private readonly EcsPoolInject<MoveComponent> _movePool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitPool.Value)
            {
                var unit = _unitPool.Pools.Inc1.Get(entity);
                
                ref var moveComponent = ref _movePool.Value.Add(entity);
                moveComponent.MoveSpeed = unit.MoveSpeed;
            }
        }
    }
}