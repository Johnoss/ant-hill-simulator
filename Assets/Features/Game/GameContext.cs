using Features.Movement;
using Features.Spawning;
using Features.Unit;
using Features.UserInput;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

namespace Features.Game

{
    public class GameContext : MonoBehaviour
    {
        [SerializeField]
        private UnitConfig _unitConfig;

        private EcsSystems _systems;

        private void Start()
        {
            var world = new EcsWorld();

            _systems = new EcsSystems(world);

            _systems
                .AddWorld(new EcsWorld(), Idents.Worlds.Events);
            
            _systems
                .Add(new KeyboardInputSystem())
                .Add(new SpawnUnitSystem())
                .Add(new StartMovingSystem())
                .Add(new RotateSystem())
                .Add(new MoveSystem())
                .DelHere<MoveCommand>()
                .DelHere<SpawnUnitEvent>(Idents.Worlds.Events)
                .Inject(_unitConfig)
                .Init();
        }

        private void Update()
        {
            _systems.Run();
        }

        private void OnDestroy()
        {
            _systems?.Destroy();
            _systems?.GetWorld().Destroy();
            _systems = null;
        }
    }
}