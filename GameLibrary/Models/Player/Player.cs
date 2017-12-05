using GameLibrary.Enums;
using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;

namespace GameLibrary.Models.Player
{
    public class Player : IPlayer
    {
        public string Name { get; private set; }
        public IPoint CurrentLocation { get; private set; }
        public IPoint RespawnLocation { get; private set; }
        public IStats PlayerStats { get; private set; }
        public ICharacterMovement Movement { get; private set; }
        public ICombatActions CombatActions { get; private set; }
        /// <summary>
        /// The idea is (Enemy name, Times killed)
        /// </summary>
        public Dictionary<string, int> MonsterCollection { get; private set; } = new Dictionary<string, int>();


        public Player(string name, IPoint loc, IPoint respawn, IStats playerStats, ICharacterMovement movement, ICombatActions combatActions, Dictionary<string, int> monsters = null)
        {
            Name = name;
            CurrentLocation = loc;
            RespawnLocation = respawn;
            PlayerStats = playerStats;
            Movement = movement;
            CombatActions = combatActions;
            if (monsters != null)
                MonsterCollection = monsters;
        }

        public void PerformMovement()
        {
            Movement.PerformMovement(CurrentLocation);
            Console.Clear();
        }
        public void UpdateLocation(IPoint point)
        {
            CurrentLocation = point;
        }
        public void UpdateRespawn(IPoint point)
        {
            RespawnLocation = point;
        }
        public void Attack(bool useMagic, IEnemy enemy)
        {            
            CombatActions.Attack(new object[] { useMagic, this, enemy, ActorEnum.Player });

            CalculateExp(useMagic ? PlayerStats.M_Atk : PlayerStats.P_Atk, enemy);
        }
        public void ChargeMana(IEnemy enemy)
        {
            CombatActions.ChargeMana(new object[] { this, enemy, ActorEnum.Player });
        }
        public bool IsDefeated(IEnemy enemy)
        {
            var defeated = CombatActions.IsDefeated(PlayerStats);
            if (!defeated)
                return false;

            //Perform death stuff here
            return true;
        }

        public void OpenMonsterCollection()
        {
            foreach (var monster in MonsterCollection)
            {
                Console.WriteLine($"You have defeated {monster.Value} {monster.Key}");
            }
        }
        public void PrintStats(bool displayCombatStats = false)
        {
            Console.WriteLine("Player stats");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Current location: {CurrentLocation}");
            PlayerStats.DisplayStats(displayCombatStats);
        }
        public void ResetHealthMana()
        {
            PlayerStats.CurrentHealth = PlayerStats.MaxHealth;
            PlayerStats.CurrentMana = PlayerStats.MaxMana;
        }

        private void CalculateExp(int statToUse, IEnemy enemy)
        {
            float formula = statToUse;
            PlayerStats.CurrentExp += Convert.ToInt32(formula);

            if (enemy.IsDefeated(this))
            {
                AddToMonsterCollection(enemy);                
                var expFormula = PlayerStats.Level >= enemy.Stats.Level ?
                    GetDefeatedExp(enemy.Stats.Level, PlayerStats.Level) :
                    GetDefeatedExp(PlayerStats.Level, enemy.Stats.Level) / 10;

                PlayerStats.CurrentExp += expFormula;
                PlayerStats.Gold += enemy.Stats.Gold;
            }

            while(PlayerStats.CurrentExp >= PlayerStats.MaxExp)
            {
                PlayerStats.LevelUp();
            }
        }
        private int GetDefeatedExp(int num1, int num2)
        {
            return Convert.ToInt32(Math.Pow(2, num1 - num2) * 100);
        }
        private void ResetPlayerToRespawn()
        {
            Console.WriteLine("YOU DIED");

            ResetHealthMana();
            PlayerStats.CurrentExp = 0;
            PlayerStats.Deaths++;
            Movement.ResetPlayerToRespawn(CurrentLocation, RespawnLocation);
        }
        private void AddToMonsterCollection(IEnemy enemy)
        {
            var type = enemy.GetType().Name;
            if (MonsterCollection.ContainsKey(type))
                MonsterCollection[type]++;
            else
                MonsterCollection.Add(type, 1);
        }
    }
}
