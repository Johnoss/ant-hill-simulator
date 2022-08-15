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
        private int _antVisionRadius = 2;
        [SerializeField]
        private int _antVisionZones = 3;
        [SerializeField]
        private float _antNewDirectionRandomDeviation;
        [SerializeField]
        private float _antDropWaypointIntervalSeconds;
        [SerializeField]
        private float _dropIntervalRandomDeviation;
        [SerializeField]
        private float _antSideVisionAngles;
        [SerializeField]
        private float _antDecideIntervalSeconds;
        [SerializeField]
        private float _decideIntervalRandomDeviation;

        public UnitView AntPrefab => _antPrefab;
        public float AntSpeed => _antSpeed;
        public float AntTurnSpeed => _antTurnSpeed;
        public int AntVisionRadius => _antVisionRadius;
        public int AntVisionZones => _antVisionZones;
        public float AntNewDirectionRandomDeviation => _antNewDirectionRandomDeviation;
        public float AntSideVisionAngles => _antSideVisionAngles;

        public float GetDropIntervalSeconds()
        {
            return _antDropWaypointIntervalSeconds.AddRandomRange(_dropIntervalRandomDeviation);
        }

        public float GetDecideIntervalSeconds()
        {
            return _antDecideIntervalSeconds.AddRandomRange(_decideIntervalRandomDeviation);
        }
    }
}