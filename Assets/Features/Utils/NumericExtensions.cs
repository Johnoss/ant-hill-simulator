using System;

namespace Features.Utils
{
    public static class NumericExtensions
    {
        public static bool IsWithinRange<T>(this T comparable, T min, T max) where T : IComparable
        {
            return min.CompareTo(comparable) <= 0 && max.CompareTo(comparable) >= 0;
        }
    }
}