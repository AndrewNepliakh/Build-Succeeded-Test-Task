using UnityEngine;

namespace UI
{
    public abstract class Window : MonoBehaviour, IView
    {
        public GameObject GameObject => gameObject;

        public virtual void Show(UIViewArguments arguments)
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}