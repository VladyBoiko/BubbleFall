using UnityEngine;

namespace LevelDesign
{
    public class SkyboxRotator : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 1f;

        private float _currentRotation = 0f;

        private void Update()
        {
            _currentRotation = (_currentRotation + Time.deltaTime * _rotationSpeed) % 360f;
            SetRotation(_currentRotation);
        }

        private void SetRotation(float value)
        {
            RenderSettings.skybox.SetFloat("_Rotation", value);
        }
    }
}