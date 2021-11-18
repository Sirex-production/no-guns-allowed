using System;

namespace Support.SLS
{
    /// <summary>
    /// Class that stores all the data that can be saved
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public const int DEFAULT_CURRENT_LEVEL = 0;
        public const float DEFAULT_AIM_SENSITIVITY = 10;
        
        public SaveDataHolder<int> CurrentLevelNumber { get; } = new SaveDataHolder<int>(DEFAULT_CURRENT_LEVEL);
        public SaveDataHolder<float> AimSensitivity  { get; } = new SaveDataHolder<float>(DEFAULT_AIM_SENSITIVITY);
    }
}