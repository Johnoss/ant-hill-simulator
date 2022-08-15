using System.Collections.Generic;
using Features.Grid;
using UnityEngine;

namespace Features.Utils
{
    public static class NodeWeightExtensions
    {
        public static float GetWeightFromNode(this NodeWeights nodeWeights, int[] weightSeekPattern)
        {
            Debug.Assert(weightSeekPattern.Length == nodeWeights.Weights.Length, "Weight Seek Pattern invalid");
            
            var sum = 0f;
            for (var i = 0; i < nodeWeights.Weights.Length; i++)
            {
                sum += nodeWeights.Weights[i] * weightSeekPattern[i];
            }

            return sum;
        }
    }
}