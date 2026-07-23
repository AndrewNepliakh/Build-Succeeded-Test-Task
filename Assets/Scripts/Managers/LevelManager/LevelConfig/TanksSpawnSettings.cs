using System;
using UnityEngine;
using System.Collections.Generic;

namespace Managers
{
    [Serializable]
    public class TanksSpawnSettings
    {
        public TanksGridConfig.ColumnsCount ColumnsCount;
        public List<TanksSpawnColumn> SpawnColumns = new ();
    }

    [Serializable]
    public class TanksSpawnColumn
    {
        public Transform ColumnParent;
        public List<Transform> SpawnPoints = new ();
    }
}