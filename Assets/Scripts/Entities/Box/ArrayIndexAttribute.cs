using Managers;
using UnityEngine;

namespace Entities
{
    public class ArrayIndexAttribute : MonoBehaviour
    {
        public int X => Mathf.RoundToInt((BoxesGridConfig.Width - 1) - transform.position.x);
        public int Z => Mathf.RoundToInt(transform.position.z);
    }
}