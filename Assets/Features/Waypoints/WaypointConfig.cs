using System.Collections.Generic;
using UnityEngine;

namespace Features.Waypoints
{
    [CreateAssetMenu(menuName = "Create WaypointConfig", fileName = "WaypointConfig", order = 0)]
    public class WaypointConfig : ScriptableObject
    {
        [Header("Visuals")]
        [SerializeField]
        private WaypointView _waypointView;
        [SerializeField]
        private Material _templateMaterial;
        [SerializeField]
        private Gradient _waypointWeightGradient;
        [SerializeField]
        private bool _createWaypointsGameObjects;
        
        public WaypointView WaypointView => _waypointView;
        public bool CreateWaypointsGameObjects => _createWaypointsGameObjects;
        
        [Header("Weight")]
        [SerializeField]
        private Vector2Int _weightRange;
        [SerializeField]
        private float _baseWaypointWeight;

        public float BaseWaypointWeight => _baseWaypointWeight;
        
        [Header("Lifespan")]
        [SerializeField]
        private float _baseLifespanSeconds;
        
        public float BaseLifespanSeconds => _baseLifespanSeconds;

        private readonly Dictionary<float, Material> _cachedMaterials = new();

        public Material GetWaypointMaterial(float waypointWeight)
        {
            waypointWeight = Mathf.Clamp(waypointWeight, _weightRange.x, _weightRange.y);
            
            return _cachedMaterials.ContainsKey(waypointWeight)
                ? _cachedMaterials[waypointWeight]
                : CreateNewMaterial(waypointWeight);
        }

        private Material CreateNewMaterial(float waypointWeight)
        {
            var gradientRange = (float)_weightRange.y - _weightRange.x;
            var mappedWeightGradient = 1f / gradientRange * (waypointWeight - _weightRange.x);
            
            var newMaterial = new Material(_templateMaterial)
            {
                color = _waypointWeightGradient.Evaluate(mappedWeightGradient)
            };

            _cachedMaterials[waypointWeight] = newMaterial;
            
            return newMaterial;
        }
    }
}