using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;

namespace GameLibrary.Models.Tiles.Special
{
    public class POITile : ITile
    {
        public string RegionId { get; } = "Point of Interest";
        public IPoint Location { get; }
        public bool Visited { get; set; } = false;
        public POITile(IPoint loc, string regionId, bool visited = false)
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
            Console.WriteLine($"You entered a {GetType().Name} tile");
            if (!Visited)
            {
                var gainedExp = 100 * player.PlayerStats.Level;
                player.PlayerStats.CurrentExp += gainedExp;
                Console.WriteLine($"You have gained {gainedExp} experience points.\r\n");
                while(player.PlayerStats.CurrentExp >= player.PlayerStats.MaxExp)
                {
                    player.PlayerStats.LevelUp();
                }                
                Visited = true;
            }
        }

        public object[] GetAppropriateParams(params object[] args)
        {
            List<object> list = new List<object>();

            return list.ToArray();
        }
    }
}
