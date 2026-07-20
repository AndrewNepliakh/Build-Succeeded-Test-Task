using Entities;
using UnityEngine;
using System.Threading.Tasks;

namespace Managers
{
    public interface IBoxManager
    {
        void InitiateAllBoxDatasPerColumns();
        void InitiatePreallocatedBoxes();
        Task CreateBufferBoxes();
        void Initiate(Transform[] columsParents, Box[] preallocatedBoxes);
    }
}