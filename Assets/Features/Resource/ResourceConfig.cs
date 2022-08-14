using System.Collections.Generic;
using UnityEngine;

namespace Features.Resource
{
    [CreateAssetMenu(menuName = "Create ResourceConfig", fileName = "ResourceConfig", order = 0)]
    public class ResourceConfig : ScriptableObject
    {
        [Header("Visuals")]
        [SerializeField]
        private ResourceView _resourceView;
        [SerializeField]
        private List<Material> _materials;
        [SerializeField]
        private List<Mesh> _meshes;
        public ResourceView ResourceView => _resourceView;
        public List<Material> Materials => _materials;
        public List<Mesh> Meshes => _meshes;

        [Header("Balancing")]
        [SerializeField]
        private float _baseResourceAmount;
        public float BaseResourceAmount => _baseResourceAmount;

        [Header("Waypoint")]
        [SerializeField]
        private float _baseWaypointWeight;
        public float BaseWaypointWeight => _baseWaypointWeight;

    }
}