using System;
using UnityEngine;
using System.Collections.Generic;

namespace Managers
{
    [Serializable]
    public class TanksSpawnSettings
    {
        public TanksGridConfig.ColumnsCount ColumnsCount;
        public List<TanksSpawnColumn> _spawnColumns = new ();
    }

    [Serializable]
    public class TanksSpawnColumn
    {
        public Transform _columnParent;
        public List<Transform> _spawnPoint = new ();
    }
}