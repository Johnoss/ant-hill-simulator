using UnityEngine;

namespace Features.Unit
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField]
        private UnitConfig _unitConfig;
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var tr = transform;
            var fromAngle = Quaternion.Euler(0, -_unitConfig.AntAngularDeviation, 0) * tr.forward;
            UnityEditor.Handles.DrawSolidArc(tr.position, tr.up, fromAngle, _unitConfig.AntAngularDeviation * 2,
                _unitConfig.AntVisionRadius);
        }
#endif
    }
}