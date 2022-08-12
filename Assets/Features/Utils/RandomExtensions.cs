using UnityEngine;

namespace Features.Utils
{
    public static class RandomExtensions
    {
        public static bool FlipCoin()
        {
            return Random.Range(0, 2) == 0;
        }
    }
}