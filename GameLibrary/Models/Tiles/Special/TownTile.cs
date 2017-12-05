using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;

namespace GameLibrary.Models.Tiles.Special
{
    public class TownTile : ITile
    {
        public string RegionId { get; } = "Town";
        public IPoint Location { get; }
        public bool Visited { get; set; } = false;
        public TownTile(IPoint loc, string regionId, bool visited = false)
        {
            Location = loc;
            RegionId = regionId;
            Visited = visited;
        }

        public void EnteredTile(IPlayer player, params object[] args)
        {
            IEnemy enemy = null;
            IBattleManager battleManager = null;
            foreach (var arg in args)
            {
                if (arg is IEnemy)
                    enemy = (IEnemy)arg;
                else if (arg is IBattleManager)
                    battleManager = (IBattleManager)arg;
            }

            EnteredTile(player);

        }
        public void EnteredTile(IPlayer player)
        {
            if (!Visited)
                Visited = true;
            Console.WriteLine($"You entered a {GetType().Name} tile");
        }

        public object[] GetAppropriateParams(params object[] args)
        {
            List<object> list = new List<object>();

            return list.ToArray();
        }
    }
}
