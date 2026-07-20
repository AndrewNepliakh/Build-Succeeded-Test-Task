using System.Threading.Tasks;
using UnityEngine;

namespace Managers
{
    public interface IBoxManager
    {
        Task FillInitialBoxGrid();
        void Initiate(Transform[] columsParents);
    }
}