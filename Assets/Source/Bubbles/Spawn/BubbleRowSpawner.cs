using System;
using UnityEngine;

namespace Bubbles.Spawn
{
    public class BubbleRowSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BubblePool _bubblePool;
        [SerializeField] private BubbleMaterialProvider _materialProvider;
        [SerializeField] private MatchManager _matchManager;
        
        [SerializeField] private Transform _gridRoot;
        
        [Header("Settings")]
        [SerializeField] private float _bubbleSpacing = 1f;
        [SerializeField] private float _rowHeight = 1f;

        private BubbleColor[] _allColors;
        private int _colorCount;

        private int _currentRowIndex = 0;
        
        public int CurrentRowIndex => _currentRowIndex;
        public float BubbleSpacing => _bubbleSpacing;
        public BubblePool BubblePool => _bubblePool;
        
        private void Awake()
        {
            _allColors = (BubbleColor[])Enum.GetValues(typeof(BubbleColor));
            _colorCount = _allColors.Length;
        }

        public void SpawnRowFromPattern(IBubbleRowPattern pattern, int rowIndex)
        {
            int columns = pattern.GetColumnsCount(rowIndex);
            int reversedRowIndex = pattern.RowCount - 1 - rowIndex;
    
            float startX = -((columns - 1) * _bubbleSpacing) / 2f;

            var colorPattern = pattern as IBubbleColorPattern;
            BubbleColor? shapeColor = colorPattern?.ShapeColor;

            for (int col = 0; col < columns; col++)
            {
                Bubble bubble = _bubblePool.Get();
        
                bool isInShape = pattern.HasBubbleAt(reversedRowIndex, col);
                var color = isInShape && shapeColor.HasValue
                    ? shapeColor.Value
                    : GetRandomColorExcept(shapeColor);

                bubble.Initialize(color, _materialProvider, _currentRowIndex, true);
                
                bubble.transform.SetParent(_gridRoot, false);
                bubble.transform.localPosition = new Vector3(startX + col * _bubbleSpacing, _currentRowIndex * _rowHeight, 0);
                _matchManager.RegisterBubble(bubble);
            }

            _currentRowIndex++;
        }
        
        private BubbleColor GetRandomColorExcept(BubbleColor? except)
        {
            if (except == null || _colorCount <= 1)
                return _allColors[UnityEngine.Random.Range(0, _colorCount)];

            BubbleColor randomColor;
            do
            {
                randomColor = _allColors[UnityEngine.Random.Range(0, _colorCount)];
            } while (randomColor == except.Value);

            return randomColor;
        }
    }
}