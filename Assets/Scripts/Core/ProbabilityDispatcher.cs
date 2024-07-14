using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Core
{
    public interface IProbabilityItem
    {
        public float Evaluate(float progress);
    }

    public class ProbabilityItem<T>
    {
        public float probability;
        public T item;
    }

    public static class ProbabilityDispatcher
    {
        public static T LaunchProbability<T>(List<T> items, float progress) where T : IProbabilityItem
        {
            List<ProbabilityItem<T>> probabilities = new List<ProbabilityItem<T>>();
            foreach (T item in items)
            {
                float probability = Random.Range(0f, 1f) * item.Evaluate(progress);
                probabilities.Add(new ProbabilityItem<T>() { item = item, probability = probability });
            }

            return probabilities.OrderByDescending(x => x.probability).First().item;
        }
    }
}