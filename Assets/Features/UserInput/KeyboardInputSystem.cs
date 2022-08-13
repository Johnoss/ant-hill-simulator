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
        private readonly EcsPoolInject<SpawnWaypointEvent> _spawnWaypointEventPool = Idents.Worlds.Events;

        private readonly EcsCustomInject<Camera> _gameCamera;

        public void Run(IEcsSystems systems)
        {
            //TODO add mapping to config
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.LeftControl))
            {
                _spawnUnitEventPool.Value.Add(_spawnUnitEventPool.Value.GetWorld().NewEntity());
            }

            if (Input.GetMouseButtonUp(0))
            {

                var ray = _gameCamera.Value.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var raycastHit))
                {
                    var spawnWaypointEntity = _spawnWaypointEventPool.Value.GetWorld().NewEntity();
                    ref var spawnWaypointEvent =  ref _spawnWaypointEventPool.Value.Add(spawnWaypointEntity);
                    spawnWaypointEvent.Position = raycastHit.point;
                    spawnWaypointEvent.WaypointWeight = -1;
                }
            }
        }
    }
}