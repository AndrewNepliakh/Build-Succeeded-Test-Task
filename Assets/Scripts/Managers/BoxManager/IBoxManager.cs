using Entities;
using UnityEngine;

namespace Managers
{
    public interface IBoxManager
    {
        void InitiateAllBoxDatasPerColumns();
        void InitiatePreallocatedBoxes();
        void Initiate(Transform[] columsParents, Box[] preallocatedBoxes);
    }
}