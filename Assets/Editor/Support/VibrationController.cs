using MoreMountains.NiceVibrations;

namespace Support
{
    public static class VibrationController
    {
        private static VibrationMode _vibrationMode;

        private static void UniversalVibrate(HapticTypes hapticType)
        {
            switch (hapticType)
            {
                case HapticTypes.Selection:
                    MMVibrationManager.TransientHaptic(.1f, 0f);
                    break;
                case HapticTypes.Success:
                    MMVibrationManager.Haptic(HapticTypes.Success);
                    break;
                case HapticTypes.Warning:
                    MMVibrationManager.Haptic(HapticTypes.Warning);
                    break;
                case HapticTypes.Failure:
                    MMVibrationManager.Haptic(HapticTypes.Failure);
                    break;
                case HapticTypes.LightImpact:
                    MMVibrationManager.TransientHaptic(.1f, 0f);
                    break;
                case HapticTypes.MediumImpact:
                    MMVibrationManager.Haptic(HapticTypes.MediumImpact);
                    break;
                case HapticTypes.HeavyImpact:
                    MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
                    break;
                case HapticTypes.RigidImpact:
                    MMVibrationManager.TransientHaptic(.2f, 1f);
                    break;
                case HapticTypes.SoftImpact:
                    MMVibrationManager.Haptic(HapticTypes.SoftImpact);
                    break;
                case HapticTypes.None:
                    break;
            }
        }

        private static void ImprovedVibrate(HapticTypes hapticType)
        {
            MMVibrationManager.Haptic(hapticType);
        }

        public static void SetMode(VibrationMode vibrationMode) => _vibrationMode = vibrationMode; 
        
        public static void Vibrate(HapticTypes hapticType)
        {
            switch (_vibrationMode)
            {
                case VibrationMode.Disabled:
                    return;
                case VibrationMode.Universal:
                    UniversalVibrate(hapticType);
                    break;
                case VibrationMode.Improved:
                    ImprovedVibrate(hapticType);
                    break;
            }
        }
    }

    public enum VibrationMode
    {
        Disabled,
        Universal,
        Improved
    }
}