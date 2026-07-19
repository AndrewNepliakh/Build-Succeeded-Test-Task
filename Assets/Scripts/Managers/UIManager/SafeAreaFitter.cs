using UnityEngine;

namespace UI
{
    [ExecuteAlways]
    public class SafeAreaFitter : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Rect _lastSafeArea;
        private Vector2Int _lastResolution;
        private ScreenOrientation _lastOrientation;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            Apply();
        }

        private void Update()
        {
            if (_lastSafeArea != Screen.safeArea ||
                _lastResolution.x != Screen.width ||
                _lastResolution.y != Screen.height ||
                _lastOrientation != Screen.orientation)
            {
                Apply();
            }
        }

        private void Apply()
        {
            var safeArea = Screen.safeArea;

            var min = safeArea.position;
            var max = safeArea.position + safeArea.size;

            min.x /= Screen.width;
            min.y /= Screen.height;
            max.x /= Screen.width;
            max.y /= Screen.height;

            _rectTransform.anchorMin = min;
            _rectTransform.anchorMax = max;
            _rectTransform.offsetMin = Vector2.zero;
            _rectTransform.offsetMax = Vector2.zero;

            _lastSafeArea = Screen.safeArea;
            _lastResolution = new Vector2Int(Screen.width, Screen.height);
            _lastOrientation = Screen.orientation;
        }
    }
}
