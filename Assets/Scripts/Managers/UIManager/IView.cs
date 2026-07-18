using UnityEngine;

namespace UI
{
    public interface IView
    {
        GameObject GameObject { get; }
        void Show(UIViewArguments arguments);
        void Hide();
    }
}