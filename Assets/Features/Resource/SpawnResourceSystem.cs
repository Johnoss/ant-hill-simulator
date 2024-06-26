﻿using Features.Game;
using Features.Grid;
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
        private readonly EcsPoolInject<StaticPoseComponent> _staticPosePool;

        private readonly EcsCustomInject<ResourceConfig> _resourceConfig;
        private readonly EcsCustomInject<ViewPool<ResourceView>> _resourceViewPool;
        private readonly EcsCustomInject<GridService> _gridService;

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
                _staticPosePool.Value.Add(resourceEntity);

                resourceComponent.ResourceAmount = _resourceConfig.Value.BaseResourceAmount;
                waypointComponent.HomeWaypointWeight = _resourceConfig.Value.BaseWaypointWeight;
                poseComponent.Pose.position = spawnEvent.Position;
                poseComponent.Pose.rotation = Random.rotation;
                transformComponent.Transform = view.transform;
                transformComponent.Transform.position = spawnEvent.Position;
                transformComponent.Transform.rotation = poseComponent.Pose.rotation;
                
                _gridService.Value.UpdateWeights(poseComponent.Pose.position, waypointComponent);
            }
        }
    }
}