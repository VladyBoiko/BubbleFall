using System.Collections.Generic;
using UnityEngine;

namespace Bubbles.Spawn
{
    public class PatternManager : MonoBehaviour
    {
        private class PatternEntry
        {
            public IBubbleRowPattern Pattern;
            public float Weight;

            public PatternEntry(IBubbleRowPattern pattern, float weight)
            {
                Pattern = pattern;
                Weight = weight;
            }
        }
        
        [Header("References")]
        [SerializeField] private BubbleRowSpawner _rowSpawner;
        
        private readonly List<PatternEntry> _patternEntries = new();
        private Queue<IBubbleRowPattern> _patternQueue = new();
        
        private IBubbleRowPattern _currentPattern;
        private int _currentPatternRowIndex = 0;
        
        public void InitPatternEntries()
        {
            _patternEntries.Add(new PatternEntry(new HexPattern(8, 15), 0.75f));
            _patternEntries.Add(new PatternEntry(new TrianglePattern(5, 15), 0.15f));
            _patternEntries.Add(new PatternEntry(new RectanglePattern(3, 15), 0.1f));
        }
        
        public void EnqueueInitialPatterns()
        {
            _patternQueue.Enqueue(new HexPattern(10, 15));
        }
        
        public void SpawnNextRow()
        {
            if (_currentPattern == null || _currentPatternRowIndex >= _currentPattern.RowCount)
            {
                if (_patternQueue.Count == 0)
                {
                    EnqueueRandomPattern();
                }

                _currentPattern = _patternQueue.Dequeue();
                _currentPatternRowIndex = 0;
            }

            _rowSpawner.SpawnRowFromPattern(_currentPattern, _currentPatternRowIndex);
            _currentPatternRowIndex++;
        }
        
        private void EnqueueRandomPattern()
        {
            var selected = GetRandomPatternByWeight(_patternEntries);
            _patternQueue.Enqueue(selected);
        }
        
        private IBubbleRowPattern GetRandomPatternByWeight(List<PatternEntry> entries)
        {
            if (entries == null || entries.Count == 0)
            {
                Debug.LogError("No pattern entries available!");
                return null;
            }
            
            float totalWeight = 0f;
            foreach (var entry in entries)
                totalWeight += entry.Weight;

            float randomValue = Random.Range(0f, totalWeight);
            float cumulative = 0f;

            foreach (var entry in entries)
            {
                cumulative += entry.Weight;
                if (randomValue <= cumulative)
                    return entry.Pattern;
            }

            return entries[0].Pattern;
        }
    }
}