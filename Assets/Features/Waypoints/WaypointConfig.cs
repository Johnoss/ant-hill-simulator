using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Waypoints
{
    [CreateAssetMenu(menuName = "Create WaypointConfig", fileName = "WaypointConfig", order = 0)]
    public class WaypointConfig : ScriptableObject
    {
        [SerializeField]
        private WaypointView _waypointView;
        public WaypointView WaypointView => _waypointView;
        [SerializeField]
        private Material _templateMaterial;

        public Material TemplateMaterial => _templateMaterial;

        [SerializeField]
        private Gradient _waypointWeightGradient;

        [SerializeField]
        private Vector2Int _weightRange;


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