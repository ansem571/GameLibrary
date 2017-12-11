using GameLibrary.Interfaces;
using GameLibrary.Models.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameLibrary.Models.Tiles.Special
{
    public class DungeonTile : ITile
    {
        public string RegionId { get; } = "Dungeon";
        public IPoint Location { get; }
        public bool Visited { get; set; } = false;
        public bool Cleared { get; set; } = false;

        private bool _victory = false;
        private IBattleManager _battleManager;
        private readonly string _enemyAssemblyName = "GameLibrary.Models.Enemies.";
        private List<IEnemy> _enemies;
        private int _numOfEnemies;

        public DungeonTile(IPoint loc, string regionId, bool visited, bool cleared, int numOfEnemies, IBattleManager battleManager)
        {
            Location = loc ?? throw new ArgumentNullException(nameof(loc));
            _battleManager = battleManager ?? throw new ArgumentNullException(nameof(battleManager));

            RegionId = regionId;
            Visited = visited;
            Cleared = cleared;
            _numOfEnemies = numOfEnemies;
        }

        public void EnteredTile(IPlayer player)
        {
            if (!Visited)
                Visited = true;

            if (Cleared)
            {
                Console.WriteLine($"You have already cleared this {GetType().Name}");
                Console.WriteLine("Nothing left to do here, so you make camp for the day.");
                return;
            }
            Console.WriteLine($"You entered a {GetType().Name}");
            Action victory = () => _victory = true;
            Action defeat = () => _victory = false;


            foreach (var enemy in _enemies)
            {
                _battleManager.Battle(player, enemy, victory, defeat);
                Console.Clear();
                if (!_victory)
                    return;
            }

            player.ResetHealthMana();
            Cleared = true;
            Console.WriteLine("Congrats. You cleared the dungeon.\r\nPress enter to continue...");
            Console.ReadLine();
            Console.Clear();
        }

        public void SetupParamsForTile(IPlayer player)
        {
            _enemies = new List<IEnemy>();
            //Has the last enemy be the boss
            for (var i = 0; i < _numOfEnemies + 1; i++)
            {
                Type type = GetEnemyType();

                IEnemy enemy = (IEnemy)Activator.CreateInstance(type, new object[] { 1 + (player.GetCurrentStats().Level / 3), i == _numOfEnemies });

                _enemies.Add(enemy);
            }
        }

        private Type GetEnemyType()
        {
            var random = new Random(new Random().Next(100) + 1);

            var enemies = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.Namespace.Contains(_enemyAssemblyName) && !x.Name.Contains("Stats")).ToList();

            var enemyType = enemies[random.Next(enemies.Count)];
            return enemyType;
        }
    }
}
