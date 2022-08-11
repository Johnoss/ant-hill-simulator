using UnityEngine;

namespace Features.Unit
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField]
        private UnitConfig _unitConfig;
        
        private void OnDrawGizmosSelected()
        {
            var tr = transform;
            var fromAngle = Quaternion.Euler(0, -_unitConfig.AntAngularDeviation / 2, 0) * tr.forward;
            
            UnityEditor.Handles.DrawSolidArc(tr.position, tr.up, fromAngle, _unitConfig.AntAngularDeviation,
                _unitConfig.AntVisionRadius);
        }
    }
}