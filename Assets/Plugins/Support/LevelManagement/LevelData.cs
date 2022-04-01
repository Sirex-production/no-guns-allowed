using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Support
{
    [CreateAssetMenu(menuName = "Data/Support/LevelData", fileName = "NewLevelData")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private List<LevelDataContainer> levelDataContainers;
        
        public int TotalNumberOfLevels => levelDataContainers.Count;
        
        public int GetSceneIndexByLevel(int levelNumber)
        {
            return levelDataContainers[levelNumber].sceneIndex;
        }
    }

    [Serializable]
    public class LevelDataContainer
    {
        [Scene]
        [AllowNesting]
        public int sceneIndex;
    }
}