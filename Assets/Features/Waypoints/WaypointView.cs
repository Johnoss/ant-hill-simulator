using UnityEngine;

namespace Features.Waypoints
{
    public class WaypointView : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer _meshRenderer;
        public void Setup(Material waypointMaterial)
        {
            _meshRenderer.material = waypointMaterial;
        }
    }
}