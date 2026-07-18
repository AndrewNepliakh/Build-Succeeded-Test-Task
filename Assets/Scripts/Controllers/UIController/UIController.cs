using UI;
using Zenject;
using UnityEngine;

namespace Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Transform _hudContainer;
        [SerializeField] private Transform _safeAreaRoot;

        [Inject] private IUIManager _uiManager;

        private void Awake()
        {
            _uiManager.Init(_mainCanvas, _safeAreaRoot, _hudContainer);
        }
    }
}