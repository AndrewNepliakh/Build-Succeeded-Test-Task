using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public interface IUIManager : IDisposable
    {
        Canvas MainCanvas { get; }
        void Init(Canvas mainCanvas, Transform safeAreaRoot, Transform hudContainer);
        Task<T> ShowPopup<T>(UIViewArguments args = null, bool safeArea = false) where T : Window;
        Task<T> ShowHUDWindow<T>(UIViewArguments args = null, bool safeArea = false) where T : Window;
        void HideHUDWindow();
        void HideCurrentPopup();
        void HidePopup<T>() where T : Window;
        Window GetCurrentHUDWindow();
        public event Action<Window> OnWindowOpened; 
        public event Action<Window> OnWindowHide;
        Window GetPopup<T>() where T : Window;
        void HideAllViews();
    }
}