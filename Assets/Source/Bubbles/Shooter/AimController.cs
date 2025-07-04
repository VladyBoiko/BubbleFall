using UnityEngine;

namespace Bubbles.Shooter
{
    public class AimController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LineRenderer _aimLine;
        
        [Header("Aim Settings")]
        [SerializeField] private int _maxBounces = 3;
        [SerializeField] private float _maxAimDistance = 10f;
        [SerializeField] private float _maxAimAngle = 65f;
        [SerializeField] private LayerMask _bounceLayerMask; 
        [SerializeField] private LayerMask _stopAtLayerMask;
        
        private Vector2 _aimScreenPosition;
        private Camera _mainCamera;
        private Transform _shootPoint;

        private void Awake()
        {
            _aimLine.enabled = false;
            _mainCamera = Camera.main;
        }

        public void Initialize(Transform shootPoint)
        {
            _shootPoint = shootPoint;
        }

        public void SetAimScreenPosition(Vector2 screenPos)
        {
            _aimScreenPosition = screenPos;
            UpdateAimLine();
        }

        public void EnableAimLine(bool enable)
        {
            _aimLine.enabled = enable;
        }

        public void SetColor(Color color)
        {
            if (_aimLine == null) return;
            _aimLine.startColor = color;
            _aimLine.endColor = color;
        }
        
        private void UpdateAimLine()
        {
            Vector3 worldMouse = GetWorldMousePositionOnPlane(_shootPoint.position.z);
            Vector3 startPos = _shootPoint.position;

            Vector3 direction3D = (worldMouse - startPos).normalized;
            Vector2 dir2D = new Vector2(direction3D.x, direction3D.y);

            float angle = Mathf.Clamp(Vector2.SignedAngle(Vector2.up, dir2D), -_maxAimAngle, _maxAimAngle);
            Vector2 clampedDir2D = Quaternion.Euler(0, 0, angle) * Vector2.up;

            Vector3 clampedDirection = new Vector3(clampedDir2D.x, clampedDir2D.y, 0).normalized;

            DrawAimLine(startPos, clampedDirection);
        }

        private void DrawAimLine(Vector3 startPos, Vector3 direction)
        {
            const float startOffset = 0.05f;

            Vector3 currentPos = startPos + direction * startOffset;
            Vector3 currentDir = direction;

            _aimLine.positionCount = 1;
            _aimLine.SetPosition(0, startPos);

            int bounces = 0;

            while (bounces < _maxBounces)
            {
                if (Physics.Raycast(currentPos, currentDir, out RaycastHit hit, _maxAimDistance, _bounceLayerMask | _stopAtLayerMask))
                {
                    _aimLine.positionCount += 1;
                    _aimLine.SetPosition(_aimLine.positionCount - 1, hit.point);

                    if ((_stopAtLayerMask.value & (1 << hit.collider.gameObject.layer)) != 0)
                        break;

                    currentPos = hit.point + hit.normal * startOffset;
                    currentDir = Vector3.Reflect(currentDir, hit.normal);
                    bounces++;
                }
                else
                {
                    Vector3 endPos = currentPos + currentDir * _maxAimDistance;
                    _aimLine.positionCount += 1;
                    _aimLine.SetPosition(_aimLine.positionCount - 1, endPos);
                    break;
                }
            }
        }

        public Vector3 GetClampedShootDirection()
        {
            Vector3 worldMouse = GetWorldMousePositionOnPlane(_shootPoint.position.z);
            Vector3 startPos = _shootPoint.position;

            Vector3 direction3D = (worldMouse - startPos).normalized;
            Vector2 dir2D = new Vector2(direction3D.x, direction3D.y);
            float angle = Mathf.Clamp(Vector2.SignedAngle(Vector2.up, dir2D), -_maxAimAngle, _maxAimAngle);
            Vector2 clampedDir2D = Quaternion.Euler(0, 0, angle) * Vector2.up;
            return new Vector3(clampedDir2D.x, clampedDir2D.y, 0).normalized;
        }

        private Vector3 GetWorldMousePositionOnPlane(float zPlane)
        {
            Ray ray = _mainCamera.ScreenPointToRay(_aimScreenPosition);
            Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, zPlane));

            if (plane.Raycast(ray, out float enter))
                return ray.GetPoint(enter);

            return _shootPoint.position;
        }
    }
}
