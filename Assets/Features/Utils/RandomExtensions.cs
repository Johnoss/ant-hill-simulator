using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Utils
{
    public static class RandomExtensions
    {
        public static bool FlipCoin()
        {
            return Random.Range(0, 2) == 0;
        }

        public static int GetWeighedRandomIndex(this float[] weights, float baseChance)
        {
            //TODO question should be optimized?
            if (weights.AreAllElementsEqual())
            {
                return weights.GetRandomElementIndex();
            }
            var minWeight = weights.Min();
            
            var sortedWeights = weights
                .Select((weight, index) => new KeyValuePair<float,int>(weight - minWeight + baseChance, index))
                .OrderBy(weightPair => weightPair.Key)
                .ToList();

            var random = Random.Range(0, sortedWeights.Sum(weight => weight.Key) - sortedWeights[0].Key);

            var index = 0;

            var runningTotal = 0f;
            
            while (true)
            {
                var iterationWeight = sortedWeights[index].Key;
                if (random <= iterationWeight + runningTotal)
                {
                    return sortedWeights[index].Value;
                }

                runningTotal += iterationWeight;
                index++;
            }
        }

        public static float AddRandomRange(this float origin, float deviation)
        {
            return origin + Random.Range(-deviation, deviation);
        }
        
        private static int GetRandomElementIndex<T>(this IEnumerable<T> collection)
        {
            var randomElementIndex = Random.Range(0, collection.Count());
            return randomElementIndex;
        }
    }
}