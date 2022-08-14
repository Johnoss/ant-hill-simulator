using Features.Game;
using Features.Position;
using Features.Timer;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Features.Lifespan
{
    public class LifespanExpiredSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TimerExpiredCommand, LifespanTimerComponent>> _lifespanTimerExpiredPool =
            Idents.Worlds.Timer;

        private readonly EcsPoolInject<TransformComponent> _transformPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var timerEntity in _lifespanTimerExpiredPool.Value)
            {
                var timerWorld = _lifespanTimerExpiredPool.Value.GetWorld();

                DeleteLifespanEntity(_lifespanTimerExpiredPool.Pools.Inc2.Get(timerEntity));
                
                timerWorld.DelEntity(timerEntity);
            }
        }

        private void DeleteLifespanEntity(LifespanTimerComponent lifespanTimerComponent)
        {
            var lifespanEntity = lifespanTimerComponent.LifespanEntity;
            if (_transformPool.Value.Has(lifespanEntity))
            {
                Object.Destroy(_transformPool.Value.Get(lifespanEntity).Transform.gameObject);
            }

            lifespanTimerComponent.World.DelEntity(lifespanEntity);
        }
    }
}