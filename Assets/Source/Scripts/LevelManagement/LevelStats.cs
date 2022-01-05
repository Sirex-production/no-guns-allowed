using UnityEngine;

namespace Ingame
{
    public class LevelStats : MonoBehaviour
    {
        private DamageType _deathDamageType = DamageType.None;
        private uint _enemiesKilled = 0;
        private float _timeWhenLevelStarts = 0;

        public float TimePassedFromTheBeginningOfTheLevel => Time.time - _timeWhenLevelStarts;

        public (DamageType deathDamageType, uint enemiesKilled, float TimePassedFromTheBeginingOfTheLevel) StatsPack
            => (_deathDamageType, _enemiesKilled, TimePassedFromTheBeginningOfTheLevel);

        public void StartLevel()
        {
            _timeWhenLevelStarts = Time.time;
            _enemiesKilled = 0;
            _deathDamageType = DamageType.None;
        }

        public void AddKilledEnemyToStats()
        {
            _enemiesKilled++;
        }

        public void AddPlayerDeathToStats(DamageType damageType)
        {
            _deathDamageType = damageType;
        }
    }
}