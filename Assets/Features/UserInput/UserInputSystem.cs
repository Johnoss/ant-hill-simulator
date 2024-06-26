﻿using Features.Game;
using Features.Resource;
using Features.Unit;
using Features.Waypoints;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Features.UserInput
{
    public class UserInputSystem : IEcsRunSystem
    {
        private readonly EcsPoolInject<SpawnUnitEvent> _spawnUnitEventPool = Idents.Worlds.Events;
        private readonly EcsPoolInject<SpawnWaypointEvent> _spawnWaypointEventPool = Idents.Worlds.Events;
        private readonly EcsPoolInject<SpawnResourceEvent> _spawnResourceEventPool = Idents.Worlds.Events;

        private readonly EcsCustomInject<Camera> _gameCamera;

        public void Run(IEcsSystems systems)
        {
            //TODO add mapping to config
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnUnit();
            }
            
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                for (var i = 0; i < 500; i++)
                {
                    SpawnUnit();
                }
            }

            if (Input.GetMouseButton(0))
            {
                var ray = _gameCamera.Value.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var raycastHit))
                {
                    var spawnWaypointEntity = _spawnWaypointEventPool.Value.GetWorld().NewEntity();
                    ref var spawnWaypointEvent =  ref _spawnWaypointEventPool.Value.Add(spawnWaypointEntity);
                    spawnWaypointEvent.Position = raycastHit.point;
                    spawnWaypointEvent.Position.y = 0;
                    spawnWaypointEvent.GoalWaypointWeight = 60;
                }
            }

            if (Input.GetMouseButton(1))
            {
                var ray = _gameCamera.Value.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var raycastHit))
                {
                    var spawnResourceEntity = _spawnResourceEventPool.Value.GetWorld().NewEntity();
                    ref var spawnResourceEvent =  ref _spawnResourceEventPool.Value.Add(spawnResourceEntity);
                    spawnResourceEvent.Position = raycastHit.point;
                    spawnResourceEvent.Position.y = 0;
                }
            }
        }

        private void SpawnUnit()
        {
            _spawnUnitEventPool.Value.Add(_spawnUnitEventPool.Value.GetWorld().NewEntity());
        }
    }
}