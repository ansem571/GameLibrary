using GameLibrary.Interfaces;
using System;

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
                var gainedExp = 100 * player.GetCurrentStats().Level;
                player.GetAllCurrentStats().CurrentExp += gainedExp;
                Console.WriteLine($"You have gained {gainedExp} experience points.\r\n");
                while(player.GetAllCurrentStats().CurrentExp >= player.GetAllCurrentStats().MaxExp)
                {
                    player.GetAllCurrentStats().LevelUp();
                }                
                Visited = true;
            }
        }

        /// <summary>
        /// Unused
        /// </summary>
        /// <param name="player"></param>
        public void SetupParamsForTile(IPlayer player) { }
    }
}
