using GameLibrary.Interfaces;
using GameLibrary.Models.Tiles.Special;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameLibrary.Models.VictoryConditions
{
    public class DungeonVictoryCondition : IVictoryCondition
    {
        private IMap _map { get; }
        private List<DungeonTile> Dungeons = new List<DungeonTile>();
        private IPlayer _player { get; }
        public DungeonVictoryCondition(IMap map, IPlayer player, List<DungeonTile> dungeons)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _player = player ?? throw new ArgumentNullException(nameof(_player));
            Dungeons = dungeons ?? throw new ArgumentException($"{nameof(dungeons)} is null", nameof(dungeons));
            if(!Dungeons.Any())
                throw new ArgumentException($"{nameof(dungeons)} is empty", nameof(dungeons));
        }
        public bool VictoryConditionAchieved()
        {
            return Dungeons.All(x => x.Cleared);
        }

        public void DisplayVictoryMessage()
        {
            Console.WriteLine($"Congratulations {_player.GetName()}, You are victories in clearing all the dungeons on the {nameof(_map)}");
            Console.WriteLine("We will be adding more content in future patches. Stay tuned.");
            Thread.Sleep(3000);
            Console.WriteLine();
        }
    }
}
