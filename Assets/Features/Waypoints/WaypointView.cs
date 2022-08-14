using Features.ViewPool;
using UnityEngine;

namespace Features.Waypoints
{
    public class WaypointView : MonoBehaviour, IPoolable<WaypointView>
    {
        [SerializeField]
        private MeshRenderer _meshRenderer;

        private ViewPool<WaypointView> _pool;

        public void Setup(Material waypointMaterial)
        {
            _meshRenderer.material = waypointMaterial;
        }

        private void OnDisable()
        {
            _pool.AddToPool(this);
        }

        public void SetupPool(ViewPool<WaypointView> pool)
        {
            _pool = pool;
        }
    }
}