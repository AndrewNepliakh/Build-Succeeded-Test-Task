using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LosePopup : Window
    {
        [SerializeField] private Button _continueButton;

        public event Action OnContinueButtonClicked;
        
        public override void Show(UIViewArguments arguments)
        {
            base.Show(arguments);

            UpdateAppearance();
            
            _continueButton.onClick.AddListener(OnContinueClickedHandled);
        }
        
        private void OnContinueClickedHandled()
        {
            OnContinueButtonClicked?.Invoke();
        }

        private void UpdateAppearance()
        {
        }
        
        public override void Hide()
        {
            base.Hide();
            
            _continueButton.onClick.RemoveListener(OnContinueClickedHandled);
        }
    }
}