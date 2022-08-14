using Features.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Unit
{
    [CreateAssetMenu(menuName = "Create UnitConfig", fileName = "UnitConfig", order = 0)]
    public class UnitConfig : ScriptableObject
    {
        [Header("Ant")]
        [SerializeField]
        private UnitView _antPrefab;
        [SerializeField]
        private float _antSpeed = 3f;
        [SerializeField]
        private float _antTurnSpeed = 2f;
        [SerializeField]
        private float _antVisionRadius = 5f;
        [SerializeField]
        private float _antAngularDeviation = 30f;
        [SerializeField]
        private int _antVisionZones = 3;
        [SerializeField]
        private float _antNewDirectionRandomDeviation;
        [SerializeField]
        private float _antDropWaypointFrequencySeconds;
        [SerializeField]
        private float _dropIntervalRandomDeviation;

        public UnitView AntPrefab => _antPrefab;
        public float AntSpeed => _antSpeed;
        public float AntTurnSpeed => _antTurnSpeed;
        public float AntVisionRadius => _antVisionRadius;
        public float AntAngularDeviation => _antAngularDeviation;
        public int AntVisionZones => _antVisionZones;
        public float AntNewDirectionRandomDeviation => _antNewDirectionRandomDeviation;

        public float GetDropIntervalSeconds()
        {
            return _antDropWaypointFrequencySeconds.AddRandomRange(
                _dropIntervalRandomDeviation);
        }
        
    }
}