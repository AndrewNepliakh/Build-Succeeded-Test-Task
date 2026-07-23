#if UNITY_EDITOR

using System.Collections.Generic;
using Controllers;
using UnityEngine;
using Managers;

public class ColumnsTanksCollector : MonoBehaviour
{
    [SerializeField] private GameplayController _gameplayController;

    [ContextMenu("Collect Tanks Spawn Settings")]
    private void Collect()
    {
        var result = new List<TanksSpawnSettings>();

        foreach (Transform container in transform)
        {
            var settings = new TanksSpawnSettings();

            settings.ColumnsCount =
                (TanksGridConfig.ColumnsCount)container.childCount;

            settings._spawnColumns = new List<TanksSpawnColumn>();

            foreach (Transform columnParent in container)
            {
                var spawnColumn = new TanksSpawnColumn
                {
                    _columnParent = columnParent,
                    _spawnPoints = new List<Transform>()
                };

                for (var i = 0; i < columnParent.childCount; i++)
                {
                    spawnColumn._spawnPoints.Add(columnParent.GetChild(i));
                }

                settings._spawnColumns.Add(spawnColumn);
            }

            result.Add(settings);
        }

        _gameplayController.SetTanksSpawnSettings(result);

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(_gameplayController);
#endif

        Debug.Log($"Collected {result.Count} TanksSpawnSettings.");
    }
}

#endif