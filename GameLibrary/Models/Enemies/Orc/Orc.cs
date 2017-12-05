using GameLibrary.Enums;
using GameLibrary.Interfaces;
using System;

namespace GameLibrary.Models.Enemies.Orc
{
    public class Orc : IEnemy
    {
        public bool IsBoss { get; } = false;
        public string Name { get; }
        public IStats Stats { get; }
        public ICombatActions CombatActions { get; }

        public Orc(bool isBoss = false) : this("Orc", new OrcStats(), new CombatActions(), isBoss) { }

        public Orc(int level, bool isBoss = false) : this("Orc", new OrcStats() { Level = level }, new CombatActions(), isBoss) { }

        public Orc(string name, IStats stats, ICombatActions actions, bool isBoss = false)
        {
            Name = name;
            Stats = stats;
            CombatActions = actions;
            IsBoss = isBoss;
        }

        public void Attack(bool useMagic, IPlayer player)
        {
            CombatActions.Attack(new object[] { useMagic, this, player, ActorEnum.Enemy });
        }
        public void ChargeMana(IPlayer player)
        {
            CombatActions.ChargeMana(new object[] { this, player, ActorEnum.Enemy });
        }

        public bool IsDefeated(IPlayer player)
        {
            var defeated = CombatActions.IsDefeated(Stats);
            if (!defeated)
                return false;
            Stats.CurrentHealth = 0;
            var name = (IsBoss ? "Boss" : "") + Name + " " + GetType().Name;
            Console.WriteLine($"You have defeated {name}");
            //Perform death stuff here
            return true;
        }
    }
}
