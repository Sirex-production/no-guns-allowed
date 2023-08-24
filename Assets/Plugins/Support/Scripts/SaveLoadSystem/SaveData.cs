using System;

namespace Support.SLS
{
    /// <summary>
    /// Class that stores all the data that can be saved
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public const int DEFAULT_CURRENT_LEVEL = 1;
        public const float DEFAULT_AIM_SENSITIVITY = 5;
        public const bool DEFAULT_IS_VIBRATION_ENABLED = true;
        
        public SaveDataHolder<int> CurrentLevelNumber { get; } = new SaveDataHolder<int>(DEFAULT_CURRENT_LEVEL);
        public SaveDataHolder<int> LastUnlockedLevelNumber { get; } = new SaveDataHolder<int>(DEFAULT_CURRENT_LEVEL);
        public SaveDataHolder<float> AimSensitivity  { get; } = new SaveDataHolder<float>(DEFAULT_AIM_SENSITIVITY);
        public SaveDataHolder<bool> IsVibrationEnabled { get; } = new SaveDataHolder<bool>(DEFAULT_IS_VIBRATION_ENABLED);
    }
}