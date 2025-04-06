using UnityEngine;

namespace WinterUniverse
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private float _cursorSpeed = 1f;
        [SerializeField] private GameObject _testClickVFX;
        [SerializeField] private LayerMask _detectableMask;

        private PlayerInputActions _inputActions;
        private Vector2 _moveCameraInput;
        private Vector2 _moveCursorInput;
        private Vector2 _cursorPosition;
        private RaycastHit _cameraHit;
        private Ray _cameraRay;

        protected override void Initialize()
        {
            _inputActions = new();
        }

        private void OnEnable()
        {
            _inputActions.Enable();
            _inputActions.Camera.Interact.performed += ctx => OnInteractPerfomed();
            _cursorPosition.x = Screen.width / 2f;
            _cursorPosition.y = Screen.height / 2f;
            EventBus.CursorMoved(_cursorPosition);
        }

        private void OnDisable()
        {
            _inputActions.Camera.Interact.performed -= ctx => OnInteractPerfomed();
            _inputActions.Disable();
        }

        private void LateUpdate()
        {
            _moveCameraInput = _inputActions.Camera.MoveCamera.ReadValue<Vector2>();
            if (_moveCameraInput != Vector2.zero)
            {

            }
            _moveCursorInput = _inputActions.Camera.MoveCursor.ReadValue<Vector2>();
            if (_moveCursorInput != Vector2.zero)
            {
                _cursorPosition.x = Mathf.Clamp(_cursorPosition.x + _moveCursorInput.x * _cursorSpeed, 0f, Screen.width);
                _cursorPosition.y = Mathf.Clamp(_cursorPosition.y + _moveCursorInput.y * _cursorSpeed, 0f, Screen.height);
                EventBus.CursorMoved(_cursorPosition);
            }
        }

        private void OnInteractPerfomed()
        {
            _cameraRay = Camera.main.ScreenPointToRay(_cursorPosition);
            if (Physics.Raycast(_cameraRay, out _cameraHit, float.MaxValue, _detectableMask))
            {
                //...
                float localX = _cameraHit.point.x - _cameraHit.transform.position.x;
                float localZ = _cameraHit.point.z - _cameraHit.transform.position.z;
                Vector2 location = HexMetrics.CoordinateToOffset(localX, localZ, HexGrid.StaticInstance.HexSize, HexGrid.StaticInstance.HexOrientation);
                Vector3 center = HexMetrics.Center(HexGrid.StaticInstance.HexSize, (int)location.x, (int)location.y, HexGrid.StaticInstance.HexOrientation);
                Instantiate(_testClickVFX, center, Quaternion.identity);
            }
        }
    }
}