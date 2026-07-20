using Managers;
using UnityEngine;

namespace Entities
{
    public class ArrayIndexMarker : MonoBehaviour
    {
        [SerializeField] private Vector2Int _arrayIndex;

        public Vector2Int ArrayIndex => _arrayIndex;

        [ContextMenu("Update Array Index From Position")]
        private void UpdateArrayIndexFromPosition()
        {
            var position = transform.position;

            _arrayIndex = new Vector2Int(
                Mathf.RoundToInt((BoxesGridConfig.Width - 1) - position.x),
                Mathf.RoundToInt(position.z));
        }
    }
}