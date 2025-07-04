using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bubbles
{
    [Serializable]
    [CreateAssetMenu(fileName = "BubbleMaterialProvider", menuName = "Data/Bubble/Color/BubbleMaterialProvider")]
    public class BubbleMaterialProvider : ScriptableObject
    {
        [Serializable]
        public struct BubbleMaterial
        {
            public BubbleColor bubbleColor;
            public Material material;
            public Color color;
        }

        [SerializeField] private BubbleMaterial[] _materials;

        private Dictionary<BubbleColor, BubbleMaterial> _materialMap;

        private void OnEnable()
        {
            _materialMap = new Dictionary<BubbleColor, BubbleMaterial>();
            foreach (var mat in _materials)
            {
                _materialMap[mat.bubbleColor] = mat;
            }
        }

        public Material GetMaterial(BubbleColor color)
        {
            return _materialMap.TryGetValue(color, out var data) ? data.material : null;
        }

        public Color GetAimColor(BubbleColor color)
        {
            return _materialMap.TryGetValue(color, out var data) ? data.color : Color.white;
        }
    }
}