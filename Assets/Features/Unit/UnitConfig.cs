using UnityEngine;

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
        
        public UnitView AntPrefab => _antPrefab;
        public float AntSpeed => _antSpeed;
        public float AntTurnSpeed => _antTurnSpeed;
    }
}