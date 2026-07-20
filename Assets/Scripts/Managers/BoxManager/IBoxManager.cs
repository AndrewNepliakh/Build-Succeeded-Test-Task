using Entities;
using UnityEngine;
using System.Threading.Tasks;

namespace Managers
{
    public interface IBoxManager
    {
        Task FillInitialBoxGrid();
        void InitiatePreallocatedBoxes();
        void Initiate(Transform[] columsParents, Box[] preallocatedBoxes);
    }
}