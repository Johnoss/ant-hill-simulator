using Features.Behaviour;
using Features.Grid;
using Features.Lifespan;
using Features.Movement;
using Features.Position;
using Features.Resource;
using Features.Timer;
using Features.Unit;
using Features.UserInput;
using Features.ViewPool;
using Features.Waypoints;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
#if UNITY_EDITOR
using Leopotam.EcsLite.UnityEditor;
#endif
using UnityEngine;

namespace Features.Game

{
    public class GameContext : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField]
        private UnitConfig _unitConfig;
        [SerializeField]
        private WaypointConfig _waypointConfig;
        [SerializeField]
        private ResourceConfig _resourceConfig;
        [SerializeField]
        private GridConfig _gridConfig;

        private EcsSystems _systems;

        private void Start()
        {
            var waypointViewPool = new ViewPool<WaypointView>(_waypointConfig.WaypointView);
            var resourceViewPool = new ViewPool<ResourceView>(_resourceConfig.ResourceView);

            var gridService = new GridService(_gridConfig);
            
            var world = new EcsWorld();

            _systems = new EcsSystems(world);

            _systems
                .AddWorld(new EcsWorld(), Idents.Worlds.Events)
                .AddWorld(new EcsWorld(), Idents.Worlds.Timer);

            _systems
                .Add(new TimerSystem())
                .Add(new UserInputSystem())
                .Add(new DropWaypointTimerSystem())
                .Add(new WaypointGridSyncSystem())
                .Add(new LifespanExpiredSystem())
                .Add(new SpawnUnitSystem())
                .Add(new SpawnWaypointSystem())
                .Add(new SpawnResourceSystem())
                .Add(new StartMovingSystem())
                .Add(new RotateSystem())
                .Add(new MoveSystem())
                .Add(new ReachedTargetSystem())
                .Add(new DecideDirectionSystem())
                .Add(new PoseSystem())
#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
                .Add(new EcsWorldDebugSystem(Idents.Worlds.Events))
#endif
                .DelHere<DecideDirectionCommand>()
                .DelHere<MoveCommand>()
                .DelHere<SpawnWaypointEvent>(Idents.Worlds.Events)
                .DelHere<SpawnUnitEvent>(Idents.Worlds.Events)
                .DelHere<SpawnResourceEvent>(Idents.Worlds.Events)
                .DelHere<UnitReachedTargetEvent>(Idents.Worlds.Events)
                .DelHere<TimerExpiredCommand>(Idents.Worlds.Timer)
                .Inject(_unitConfig, _waypointConfig, _resourceConfig, _gridConfig, gridService, waypointViewPool, resourceViewPool,
                    Camera.main)
                .Init();
        }
        
        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            _systems?.Destroy();
            _systems?.GetWorld().Destroy();
            _systems = null;
        }
    }
}