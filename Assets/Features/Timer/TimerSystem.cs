using Features.Game;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Features.Timer
{
    public class TimerSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TimerComponent>> _timerPool = Idents.Worlds.Timer;

        private readonly EcsPoolInject<TimerExpiredCommand> _expiredCommandPool = Idents.Worlds.Timer;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _timerPool.Value)
            {
                ref var timerComponent = ref _timerPool.Pools.Inc1.Get(entity);

                timerComponent.RemainingTimerSeconds -= Time.deltaTime;

                if (timerComponent.RemainingTimerSeconds >= 0)
                {
                    continue;
                }
                
                _expiredCommandPool.Value.Add(entity);
            }
        }
    }
}