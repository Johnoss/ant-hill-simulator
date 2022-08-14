using UnityEngine;

namespace Features.Utils
{
    public static class VectorExtensions
    {
        public static Vector2 ToXZPlane(this Vector3 origin)
        {
            return new Vector2(origin.x, origin.z);
        }
    }
}