using GameLibrary.Interfaces;
using GameLibrary.Models.Tiles.Special;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameLibrary.Models.VictoryConditions
{
    public class DungeonVictoryCondition : IVictoryCondition
    {
        public bool VictoryAchieved { get; private set; }
        private IMap _map { get; }
        private List<DungeonTile> Dungeons = new List<DungeonTile>();
        public DungeonVictoryCondition(IMap map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));

            foreach (var keyValuePair in _map.Grid)
            {
                if (keyValuePair.Value is DungeonTile)
                {
                    Dungeons.Add((DungeonTile)keyValuePair.Value);
                }
            }
        }
        public void VictoryConditionAchieved(params object[] args)
        {
            if (Dungeons.All(x => x.Cleared))
                VictoryAchieved = true;
        }

        public void DisplayVictoryMessage(params object[] args)
        {
            IPlayer player = null;
            foreach(var arg in args)
            {
                if (arg is IPlayer)
                    player = (IPlayer)arg;
            }

            Console.WriteLine($"Congratulations {player.Name}, You are victories in clearing all the dungeons on the {nameof(_map)}");
            Console.WriteLine("We will be adding more content in future patches. Stay tuned.");
            Thread.Sleep(3000);
            Console.WriteLine();

        }
    }
}
