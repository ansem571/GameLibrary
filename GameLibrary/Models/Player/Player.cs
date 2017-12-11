using GameLibrary.Enums;
using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;

namespace GameLibrary.Models.Player
{
    public class Player : IPlayer
    {
        private string _name;
        private IPoint _location;
        private IPoint _respawn;
        private IPlayerStats _playerStats;
        private ICharacterMovement _movement;
        private ICombatActions _combatActions;
        /// <summary>
        /// The idea is (Enemy name, Times killed)
        /// </summary>
        private Dictionary<string, int> MonsterCollection = new Dictionary<string, int>();


        public Player(string name, IPoint loc, IPoint respawn, IPlayerStats playerStats, ICharacterMovement movement, ICombatActions combatActions, Dictionary<string, int> monsters = null)
        {
            _name = name;
            _location = loc ?? throw new ArgumentNullException(nameof(loc));
            _respawn = respawn ?? throw new ArgumentNullException(nameof(respawn));
            _playerStats = playerStats ?? throw new ArgumentNullException(nameof(playerStats));
            _movement = movement ?? throw new ArgumentNullException(nameof(movement));
            _combatActions = combatActions ?? throw new ArgumentNullException(nameof(combatActions));
            if (monsters != null)
                MonsterCollection = monsters;
        }

        public string GetName()
        {
            return _name;
        }

        public IPoint GetCurrentLocation()
        {
            return _location;
        }

        public void PerformMovement()
        {
            _movement.PerformMovement();
            Console.Clear();
        }
        public void UpdateLocation(IPoint point)
        {
            _location = point;
        }
        public void UpdateRespawn(IPoint point)
        {
            _respawn = point;
        }
        public void Attack(bool useMagic, IEnemy enemy)
        {            
            _combatActions.Attack(_playerStats, enemy.GetCurrentStats(), useMagic);

            CalculateExp(useMagic ? _playerStats.M_Atk : _playerStats.P_Atk, enemy);
        }
        public void ChargeMana(IEnemy enemy)
        {
            _combatActions.ChargeMana(GetCurrentStats());
        }
        public bool IsDefeated(IEnemy enemy)
        {
            var defeated = _combatActions.IsDefeated(_playerStats);
            if (!defeated)
                return false;

            //Perform death stuff here
            return true;
        }

        public int GetCostOfSpell(string spellName = null)
        {
            return _playerStats.ManaCost;
        }
        /// <summary>
        /// Returns only IStats portion, for all player stats use 'GetAllCurrentStats'
        /// </summary>
        /// <returns></returns>
        public IStats GetCurrentStats()
        {
            return _playerStats;
        }
        /// <summary>
        /// Returns all player stats
        /// </summary>
        /// <returns></returns>
        public IPlayerStats GetAllCurrentStats()
        {        
            return _playerStats;
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
            Console.WriteLine($"Name: {_name}");
            Console.WriteLine($"Current location: {_location}");
            _playerStats.DisplayStats(displayCombatStats);
        }
        public void ResetHealthMana()
        {
            _playerStats.CurrentHealth = _playerStats.MaxHealth;
            _playerStats.CurrentMana = _playerStats.MaxMana;
        }

        private void CalculateExp(int statToUse, IEnemy enemy)
        {
            float formula = statToUse;
            _playerStats.CurrentExp += Convert.ToInt32(formula);

            if (enemy.IsDefeated(this))
            {
                AddToMonsterCollection(enemy);                
                var expFormula = _playerStats.Level >= enemy.GetCurrentStats().Level ?
                    GetDefeatedExp(enemy.GetCurrentStats().Level, _playerStats.Level) :
                    GetDefeatedExp(_playerStats.Level, enemy.GetCurrentStats().Level) / 10;

                _playerStats.CurrentExp += expFormula;
                _playerStats.Gold += enemy.GetCurrentStats().Gold;
            }

            while(_playerStats.CurrentExp >= _playerStats.MaxExp)
            {
                _playerStats.LevelUp();
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
            _playerStats.CurrentExp = 0;
            _playerStats.Deaths++;
            _movement.ResetPlayerToRespawn();
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
