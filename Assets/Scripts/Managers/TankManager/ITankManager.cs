using System.Collections.Generic;
using Entities;

namespace Managers
{
    public interface ITankManager
    {
        void Initiate(List<TanksSpawnSettings> tanksSpawnSettings, Tank[] preallocatedTanks);
        void InitiateAllTankDatasPerColumns();
        void InitiatePreallocatedTanks();
    }
}