using Features.Utils;
using Features.ViewPool;
using UnityEngine;

namespace Features.Resource
{
    public class ResourceView : MonoBehaviour, IPoolable<ResourceView>
    {
        [SerializeField]
        private MeshRenderer _meshRenderer;
        [SerializeField]
        private MeshFilter _meshFilter;
        [SerializeField]
        private  ResourceConfig _resourceConfig;
        
        private ViewPool<ResourceView> _pool;

        
        public void Setup()
        {
            _meshRenderer.material = _resourceConfig.Materials.RandomElement();
            _meshFilter.mesh = _resourceConfig.Meshes.RandomElement();
        }

        private void OnDisable()
        {
            _pool.AddToPool(this);
        }

        public void SetupPool(ViewPool<ResourceView> pool)
        {
            _pool = pool;
        }
    }
}