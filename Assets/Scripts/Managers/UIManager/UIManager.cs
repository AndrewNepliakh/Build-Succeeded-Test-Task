using System;
using Zenject;
using Managers;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIManager : IUIManager
    {
        [Inject] private IAssetsManager _assetsManager;

        private Window _currentPopup;
        private Window _currentHUDWindow;
        private Canvas _mainCanvas;
        private Transform _safeAreaRoot;
        private Transform _hudRoot;

        private readonly Dictionary<Type, Window> _popupsPool = new();
        private readonly Dictionary<Type, Window> _HUDPool = new();

        public event Action<Window> OnWindowOpened; 
        public event Action<Window> OnWindowHide;

        public Canvas MainCanvas => _mainCanvas;

        public void Init(Canvas mainCanvas, Transform safeAreaRoot, Transform hudRoot)
        {
            _mainCanvas = mainCanvas;
            _safeAreaRoot = safeAreaRoot;
            _hudRoot = hudRoot;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public async Task<T> ShowPopup<T>(UIViewArguments args = null, bool safeArea = false) where T : Window
        {
            var parent = safeArea ? _safeAreaRoot : _hudRoot;
            
            if (!_popupsPool.ContainsKey(typeof(T)))
            {
                var assetGo = _assetsManager.Instantiate<T>(
                    Vector3.one, 
                    Quaternion.identity,
                    parent);

                _currentPopup = assetGo.GetComponent<T>();
                _popupsPool.Add(typeof(T), _currentPopup);
                
                var rectTransform = _currentPopup.GetComponent<RectTransform>();
                
                SetRectTransform(rectTransform, parent);

                _currentPopup.Show(args);
                _currentPopup.transform.SetAsLastSibling();
            }
            else
            {
                if (_popupsPool.TryGetValue(typeof(T), out var window))
                {
                    _currentPopup = window;
                    
                    var rectTransform = _currentPopup.GetComponent<RectTransform>();
                
                    SetRectTransform(rectTransform, parent);
                    
                    _currentPopup.Show(args);
                    _currentPopup.transform.SetAsLastSibling();
                }
                else
                {
                    throw new NullReferenceException($"UIManager's pool doesn't contain view of type {typeof(T)}");
                }
            }

            return (T)_currentPopup;
        }

        public async Task<T> ShowHUDWindow<T>(UIViewArguments args = null, bool safeArea = false) where T : Window
        {
            if (_currentHUDWindow != null)
            {
                _currentHUDWindow.Hide();
                OnWindowHide?.Invoke(_currentHUDWindow);
            }
            
            var parent = safeArea ? _safeAreaRoot : _hudRoot;
            
            if (!_HUDPool.ContainsKey(typeof(T)))
            {
                var assetGO = _assetsManager.Instantiate<T>(
                    Vector3.zero, 
                    Quaternion.identity,
                    parent);

                _currentHUDWindow = assetGO.GetComponent<T>();
                _HUDPool.Add(typeof(T), _currentHUDWindow);
                
                var rectTransform = _currentHUDWindow.GetComponent<RectTransform>();
                
                SetRectTransform(rectTransform, parent);

                _currentHUDWindow.Show(args);
                
                OnWindowOpened?.Invoke(_currentHUDWindow);
            }
            else
            {
                if (_HUDPool.TryGetValue(typeof(T), out var window))
                {
                    _currentHUDWindow = window;
                    
                    var rectTransform = _currentHUDWindow.GetComponent<RectTransform>();
                
                    SetRectTransform(rectTransform, parent);
                    
                    _currentHUDWindow.Show(args);
                    
                    OnWindowOpened?.Invoke(_currentHUDWindow);
                }
                else
                {
                    throw new NullReferenceException($"UIManager's HUD pool doesn't contain view of type {typeof(T)}");
                }
            }

            return (T)_currentHUDWindow;
        }

        public void HideHUDWindow()
        {
            _currentHUDWindow?.Hide();
            OnWindowHide?.Invoke(_currentHUDWindow);
        }
        
        public Window GetCurrentHUDWindow() => _currentHUDWindow;

        public Window GetPopup<T>() where T : Window
        {
            if (_popupsPool.TryGetValue(typeof(T), out var uiView))
            {
                return (T)uiView;
            }
            else
            {
                throw new NullReferenceException($"UIManager's pool doesn't contain view of type {typeof(T)}");
            }

            return null;
        }

        public void HideAllViews()
        {
            foreach (var popup in _popupsPool.Values)
            {
                if (popup != null)
                {
                    popup.Hide();
                    OnWindowHide?.Invoke(popup);
                }
            }

            foreach (var hud in _HUDPool.Values)
            {
                if (hud != null)
                {
                    hud.Hide();
                    OnWindowHide?.Invoke(hud);
                }
            }

            _currentPopup = null;
            _currentHUDWindow = null;
        }

        public void HideCurrentPopup() => _currentPopup?.Hide();
        
        public void HidePopup<T>() where T : Window
        {
            if (_popupsPool.TryGetValue(typeof(T), out var uiView))
            {
                uiView.Hide();

                if (_currentPopup == uiView)
                    _currentPopup = null;
            }
            else
            {
                throw new NullReferenceException($"UIManager's pool doesn't contain view of type {typeof(T)}");
            }
        }
        
        public void Dispose()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            ClearPopups();
            ClearHUDs();
        }
        
        private void ClearPopups()
        {
            foreach (var popup in _popupsPool.Values)
            {
                if (popup != null)
                {
                    popup.Hide();
                    UnityEngine.Object.Destroy(popup.GameObject);
                }
            }

            _popupsPool.Clear();
            _currentPopup = null;
        }
        
        private void ClearHUDs()
        {
            foreach (var hud in _HUDPool.Values)
            {
                if (hud != null)
                {
                    hud.Hide();
                    UnityEngine.Object.Destroy(hud.GameObject);
                }
            }

            _HUDPool.Clear();
            _currentHUDWindow = null;
        }
        
        private void OnSceneUnloaded(Scene scene)
        {
            ClearPopups();
            ClearHUDs();
        }
        
        private void SetRectTransform(RectTransform rectTransform, Transform parent)
        {
            rectTransform.SetParent(parent, false);
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.rotation = parent.rotation;
            rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
            rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
    }
}
