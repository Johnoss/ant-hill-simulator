using UnityEngine;

namespace Features.Waypoints
{
    public class WaypointView : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer _meshRenderer;
        public void Setup(WaypointViewSetting waypointSetting)
        {
            _meshRenderer.material = waypointSetting.Material;
        }
    }
}