using Managers;
using UnityEngine;

namespace Entities
{
    public class BoxColorAttribute : MonoBehaviour, IAttribute
    {
        [SerializeField] private Box _box;
        [Space(10)]
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Material[] _materials;
        
        public void Initialize()
        {
            if (_box.BoxData.Color == BoxColor.None)
            {
                _renderer.sharedMaterial = null;
                return;
            }

            _renderer.sharedMaterial = _materials[(int)_box.BoxData.Color - 1];
        }
    }
}