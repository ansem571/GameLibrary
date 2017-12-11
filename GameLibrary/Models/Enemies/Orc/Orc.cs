using GameLibrary.Interfaces;
using System;

namespace GameLibrary.Models.Enemies.Orc
{
    public class Orc : IEnemy
    {
        private bool _isBoss { get; } = false;
        private string _name { get; }
        private IStats _stats { get; }
        private ICombatActions _combatActions { get; }

        public Orc(bool isBoss = false) : this("Orc", new OrcStats(), new CombatActions(), isBoss) { }

        public Orc(int level, bool isBoss = false) : this("Orc", new OrcStats() { Level = level }, new CombatActions(), isBoss) { }

        public Orc(string name, IStats stats, ICombatActions actions, bool isBoss = false)
        {
            _name = name;
            _stats = stats ?? throw new ArgumentNullException(nameof(stats));
            _combatActions = actions ?? throw new ArgumentNullException(nameof(actions));
            _isBoss = isBoss;
        }

        public void Attack(bool useMagic, IPlayer player)
        {
            _combatActions.Attack(_stats, player.GetCurrentStats(), useMagic);
        }
        public void ChargeMana()
        {
            _combatActions.ChargeMana(_stats);
        }

        public bool IsEnemyBoss()
        {
            return _isBoss;
        }

        public bool IsDefeated(IPlayer player)
        {
            var defeated = _combatActions.IsDefeated(_stats);
            if (!defeated)
                return false;
            _stats.CurrentHealth = 0;
            var name = (_isBoss ? "Boss" : "") + _name + " " + GetType().Name;
            Console.WriteLine($"You have defeated {name}");
            //Perform other death stuff here
            return true;
        }

        public IStats GetCurrentStats()
        {
            return _stats;
        }
    }
}
