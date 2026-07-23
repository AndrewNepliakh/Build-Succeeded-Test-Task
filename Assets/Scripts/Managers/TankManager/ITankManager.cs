using System.Collections.Generic;
using Entities;

namespace Managers
{
    public interface ITankManager
    {
        void Initiate(
            List<TanksSpawnSettings> tanksSpawnSettings,
            List<TankPlacement> tankPlacements,
            Tank[] preallocatedTanks);
        
        void InitiateAllTankDatasPerColumns();
        void InitiatePreallocatedTanks();
        void MoveToPlacement(Tank tank);
    }
}