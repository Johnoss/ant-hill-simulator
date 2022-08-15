using UnityEngine;

namespace Features.Waypoints
{
    public struct SpawnWaypointEvent
    {
        public Vector3 Position;
        public float HomeWaypointWeight;
        public float GoalWaypointWeight;
    }
}