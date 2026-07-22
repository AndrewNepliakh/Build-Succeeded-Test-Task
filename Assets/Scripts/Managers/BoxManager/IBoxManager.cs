using System;
using Entities;
using UnityEngine;

namespace Managers
{
    public interface IBoxManager
    {
        void Initiate(Transform[] columsParents, Box[] preallocatedBoxes);
        void InitiateAllBoxDatasPerColumns();
        void InitiatePreallocatedBoxes();
        void CreateBufferBoxes();
        
        event Action<Transform> OnColumnShifted;
    }
}