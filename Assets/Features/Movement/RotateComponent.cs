using UnityEngine;

namespace Features.Movement
{
    public struct RotateComponent
    {
        public float RotateSpeed;
        public Quaternion TargetRotation;
        public Transform Transform;
    }
}