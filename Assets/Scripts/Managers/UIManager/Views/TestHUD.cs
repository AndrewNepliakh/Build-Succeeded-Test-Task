using TMPro;
using Entities;
using UnityEngine;

namespace UI
{
    public class TestHUD : Window
    {
        [SerializeField] private TMP_Text _totalShootsText;
        [SerializeField] private TMP_Text _boxesDestroyedText;

        private int _totalShoots;
        private int _destroyedBoxes;
        
        public override void Show(UIViewArguments arguments)
        {
            base.Show(arguments);
            
            UpdateAppearance();

            TankShooter.OnShootStatic += OnShootStatic;
            Box.OnDespawnStatic += OnDespawnStatic;
        }

        private void OnShootStatic()
        {
            _totalShoots++;
            UpdateAppearance();
        }

        private void OnDespawnStatic()
        {
            _destroyedBoxes++;
            UpdateAppearance();
        }

        private void UpdateAppearance()
        {
            _totalShootsText.text = $"Total shoots: {_totalShoots}";
            _boxesDestroyedText.text = $"Destroyed boxes: {_destroyedBoxes}";
        }
        
        public override void Hide()
        {
            base.Hide();
            
            TankShooter.OnShootStatic -= OnShootStatic;
            Box.OnDespawnStatic -= OnDespawnStatic;
        }
    }
}