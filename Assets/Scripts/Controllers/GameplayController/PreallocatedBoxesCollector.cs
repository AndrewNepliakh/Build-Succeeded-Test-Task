#if UNITY_EDITOR

using Entities;
using UnityEditor;
using UnityEngine;

namespace Controllers
{
    public class PreallocatedBoxesCollector : MonoBehaviour
    {
        [SerializeField] private GameplayController _gameplayController;
        
        private Box[] _preallocatedBoxes = new Box[100];

        [ContextMenu("Collect Preallocated Boxes")]
        private void CollectPreallocatedBoxes()
        {
            const int MinRow = 0;
            const int MaxRow = 9;

            int index = 0;

            for (int row = MinRow; row <= MaxRow; row++)
            {
                for (int column = 0; column < transform.childCount; column++)
                {
                    Transform boxSpawnParent = transform.GetChild(column);

                    Box box = null;

                    foreach (Transform child in boxSpawnParent)
                    {
                        if (child.name == $"BoxPreAlloc ({row})")
                        {
                            box = child.GetComponent<Box>();
                            break;
                        }
                    }

                    _preallocatedBoxes[index++] = box;
                }
            }

            _gameplayController.SetPreallocatedBoxes(_preallocatedBoxes);
            
            EditorUtility.SetDirty(this);
        }
    }
}

#endif