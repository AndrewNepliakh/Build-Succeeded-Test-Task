using System;
using Entities;
using UnityEngine;

namespace Managers
{
    public interface IBoxManager
    {
        ColumnShifter[] GetColumns();
        void Initiate(ColumnShifter[] columsParents, Box[] preallocatedBoxes);
        void InitiateAllBoxDatasPerColumns();
        void InitiatePreallocatedBoxes();
        void CreateBufferBoxes();
        
        event Action<Transform> OnColumnShifted;
    }
}