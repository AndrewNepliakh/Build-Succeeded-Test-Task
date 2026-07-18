using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Managers
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "Configs/LevelsConfig")]
    public class LevelsConfig : ScriptableObject
    {
        [SerializeField] private List<LevelConfigData> _levelConfigDatas;
        
        public List<LevelConfigData> LevelConfigDatas => _levelConfigDatas;
    }
    
    [Serializable]
    public class LevelConfigData
    {
        [SerializeField, TableList(ShowIndexLabels = true)] 
        public LevelConfig LevelConfig;
    }

    [Serializable]
    public class LevelConfig
    {
        
    }
}