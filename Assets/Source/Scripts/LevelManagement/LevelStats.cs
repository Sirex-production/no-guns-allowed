using UnityEngine;

namespace Ingame
{
    public class LevelStats
    {
        private DamageType _deathDamageType = DamageType.None;
        private uint _enemiesKilled = 0;
        private float _timeWhenLevelStarts = 0;
        private float _timePassedFromTheBeginningOfTheLevel = 0;

        public (DamageType deathDamageType, uint enemiesKilled, float timePassedFromTheBeginingOfTheLevel) StatsPack
            => (_deathDamageType, _enemiesKilled, Time.time - _timeWhenLevelStarts);

        public void StartLevel()
        {
            _deathDamageType = DamageType.None;
            _enemiesKilled = 0;
            _timeWhenLevelStarts = Time.time;
            _timePassedFromTheBeginningOfTheLevel = 0;
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