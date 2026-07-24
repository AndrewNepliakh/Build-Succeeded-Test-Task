using Zenject;
using Services;
using Managers;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace Entities
{
    public class BoxStackInitializer : MonoBehaviour, IInitializer
    {
        [Inject] private IPoolService _poolService;
        [Inject] private IBoxManager _boxManager;

        [SerializeField] private Box _box;
        [SerializeField] private BoxHitReceiver _boxHitReceiver;
        [SerializeField] private List<BoxVisual> _boxVisuals = new();

        private int _currentExtraBoxes;

        public void Initialize()
        {
            _boxHitReceiver.OnHit += OnHit;
            _boxManager.OnColumnShifted += OnColumnShifted;

            InitialShowExtraBoxes();
        }

        private void OnColumnShifted(Transform parentColumn)
        {
            if (parentColumn != _box.ParentColumn) return;
            
            RefreshExtraBoxes();
        }

        private void InitialShowExtraBoxes()
        {
            _currentExtraBoxes = Mathf.Min(_box.BoxData.StackHeight - 1, _boxVisuals.Count);
            
            if (transform.position.z < 0) return;
            
            foreach (var visual in _boxVisuals)
                visual.gameObject.SetActive(false);
            
            for (var i = 0; i < _currentExtraBoxes; i++)
                _boxVisuals[i].gameObject.SetActive(true);
        }

        private void RefreshExtraBoxes()
        {
            if (!Mathf.Approximately(transform.position.z, 0f)) return;
            
            for (var i = 0; i < _currentExtraBoxes; i++)
                _boxVisuals[i].gameObject.SetActive(true);
        }

        private void OnHit()
        {
            if (_currentExtraBoxes > 0)
            {
                _currentExtraBoxes--;
                _boxVisuals[_currentExtraBoxes].gameObject.SetActive(false);
                
                transform.DOKill();
                
                var position = transform.position;
                position.y += 1f;
                transform.position = position;
                
                transform.DOMoveY(0f, 0.1f)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        var finalPosition = transform.position;
                        finalPosition.y = 0f;
                        transform.position = finalPosition;
                    });
                
                return;
            }
            
            _poolService.Despawn(_box);
        }

        private void OnDisable()
        {
            _boxHitReceiver.OnHit -= OnHit;
            _boxManager.OnColumnShifted -= OnColumnShifted;
        }
    }
}