using Features.Game;
using Features.Timer;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Features.Behaviour
{
    public class DecideDirectionTimerSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TimerExpiredCommand, TimerComponent, DecideTimerComponent>> _timerPool =
            Idents.Worlds.Timer;

        private readonly EcsPoolInject<DecideDirectionCommand> _decideDirectionPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _timerPool.Value)
            {
                var decideTimerComponent = _timerPool.Pools.Inc3.Get(entity);
                ref var timerComponent = ref _timerPool.Pools.Inc2.Get(entity);

                _decideDirectionPool.Value.Add(decideTimerComponent.Entity);

                timerComponent.RemainingTimerSeconds = timerComponent.DefaultSeconds;
            }
        }
    }
}