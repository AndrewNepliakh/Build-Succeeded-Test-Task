using System.Collections.Generic;

namespace Managers
{
    public interface ILevelManager
    {
        List<BoxesGridConfig> GetBoxesGridConfigsOfCurrentLevel();
    }
}