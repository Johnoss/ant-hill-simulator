using Features.Game;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Features.Behaviour
{
    //TODO question - this system only processes events and pushes commands to be processed in another system (DecideDirectionSystem) - does that look right?

    public class ReachedTargetSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<UnitReachedTargetEvent>> _reachedTargetEvents = Idents.Worlds.Events;

        private readonly EcsPoolInject<DecideDirectionCommand> _decideDirectionPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _reachedTargetEvents.Value)
            {
                var eventComponent = _reachedTargetEvents.Pools.Inc1.Get(entity);

                ref var decideCommand = ref _decideDirectionPool.Value.Add(eventComponent.UnitEntity);

                decideCommand.DecidingEntity = eventComponent.UnitEntity;
            }
        }
    }
}