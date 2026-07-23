using TMPro;
using UnityEngine;

namespace Entities
{
    public class TankIndicatorAttribute : MonoBehaviour, IAttribute
    {
        [SerializeField] private Tank _tank;
        [SerializeField] private TMP_Text _indicatorText;
        
        public void Initialize()
        {
            UpdateIndicator();
        }

        private void UpdateIndicator()
        {
            _indicatorText.text = _tank.TankData.ShootsCount.ToString();
        }
    }
}