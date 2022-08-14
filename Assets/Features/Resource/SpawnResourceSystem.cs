using Features.Game;
using Features.Position;
using Features.ViewPool;
using Features.Waypoints;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Features.Resource
{
    public class SpawnResourceSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<SpawnResourceEvent>> _spawnResourcePool = Idents.Worlds.Events;

        private readonly EcsPoolInject<ResourceComponent> _resourcePool;
        private readonly EcsPoolInject<WaypointComponent> _waypointPool;
        private readonly EcsPoolInject<PoseComponent> _posePool;
        private readonly EcsPoolInject<TransformComponent> _transformPool;

        private readonly EcsCustomInject<ResourceConfig> _resourceConfig;
        private readonly EcsCustomInject<ViewPool<ResourceView>> _resourceViewPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _spawnResourcePool.Value)
            {
                var spawnEvent = _spawnResourcePool.Pools.Inc1.Get(entity);

                var resourceEntity = _resourcePool.Value.GetWorld().NewEntity();

                var view = _resourceViewPool.Value.GetOrCreate();
                view.Setup();
                
                ref var resourceComponent = ref _resourcePool.Value.Add(resourceEntity);
                ref var waypointComponent = ref _waypointPool.Value.Add(resourceEntity);
                ref var poseComponent = ref _posePool.Value.Add(resourceEntity);
                ref var transformComponent = ref _transformPool.Value.Add(resourceEntity);

                resourceComponent.ResourceAmount = _resourceConfig.Value.BaseResourceAmount;
                waypointComponent.WaypointWeight = _resourceConfig.Value.BaseWaypointWeight;
                poseComponent.Pose.position = spawnEvent.Position;
                poseComponent.Pose.rotation = Random.rotation;
                transformComponent.Transform = view.transform;
            }
        }
    }
}