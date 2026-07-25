using Entities;
using UnityEngine;
using System.Threading.Tasks;

namespace Services
{
    public interface ITargetFindService
    {
        Task<Box> FindTarget(Tank tank, Vector3 position);
    }
}