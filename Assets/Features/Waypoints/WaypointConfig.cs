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
        private List<WaypointViewSetting> _waypointViewSetting;


        public WaypointViewSetting GetWaypointViewSetting( WaypointType type)
        {
            return _waypointViewSetting.First(setting => setting.Type == type);
        }
    }
}