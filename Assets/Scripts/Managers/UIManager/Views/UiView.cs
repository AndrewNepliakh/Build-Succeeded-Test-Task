using UnityEngine;

namespace UI
{
    public abstract class UiView : MonoBehaviour
    {
        public abstract void Init<T>(ViewData<T> viewData);
    }
}