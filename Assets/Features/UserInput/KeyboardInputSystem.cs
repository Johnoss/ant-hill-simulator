using Features.Game;
using Features.Spawning;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Features.UserInput
{
    public class KeyboardInputSystem : IEcsRunSystem
    {
        private readonly EcsPoolInject<SpawnUnitEvent> _spawnUnitEventPool = Idents.Worlds.Events;

        public void Run(IEcsSystems systems)
        {
            //TODO add mapping to config
            if (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.LeftControl))
            {
                return;
            }

            _spawnUnitEventPool.Value.Add(_spawnUnitEventPool.Value.GetWorld().NewEntity());

        }
    }
}