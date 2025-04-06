using UnityEngine;
using UnityEngine.UI;

namespace WinterUniverse
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Image _virtualCursorImage;

        protected override void Initialize()
        {
            base.Initialize();
            HideHardwareCursor();
            ShowVirtualCursor();
        }

        private void OnEnable()
        {
            EventBus.OnCursorMoved += OnCursorMoved;
        }

        private void OnDisable()
        {
            EventBus.OnCursorMoved -= OnCursorMoved;
        }

        public void ShowVirtualCursor()
        {
            _virtualCursorImage.gameObject.SetActive(true);
        }

        public void HideVirtualCursor()
        {
            _virtualCursorImage.gameObject.SetActive(false);
        }

        public void ShowHardwareCursor()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        public void HideHardwareCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnCursorMoved(Vector2 position)
        {
            _virtualCursorImage.rectTransform.anchoredPosition = position;
        }
    }
}