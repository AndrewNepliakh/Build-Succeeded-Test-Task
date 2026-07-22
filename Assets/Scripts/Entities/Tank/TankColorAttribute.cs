using UnityEngine;
using System.Collections.Generic;

namespace Entities
{
    public class TankColorAttribute : MonoBehaviour, IAttribute
    {
        [SerializeField] private Tank _tank;
        [Space(10)]
        [SerializeField] private List<MeshRenderer> _renderers = new List<MeshRenderer>();
        [SerializeField] private Material[] _materials;
        
        public void Initialize()
        {
            if (_tank.TankData.Color == BoxColor.None)
            {
                _renderers.ForEach(x => x.sharedMaterial = null);
                return;
            }

            _renderers.ForEach(x => x.sharedMaterial = _materials[(int)_tank.TankData.Color - 1]);
        }
    }
}