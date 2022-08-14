using System;
using System.Collections.Generic;

namespace Features.Utils
{
    public static class CollectionExtensions
    {
        public static bool AreAllElementsEqual<T>(this T[] collection) where T : IComparable
        {
            for (var i = 1; i < collection.Length; i++)
            {
                if (!collection[i].Equals(collection[0]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}