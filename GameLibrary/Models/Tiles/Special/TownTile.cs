using GameLibrary.Interfaces;
using System;

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

        public void EnteredTile(IPlayer player)
        {
            if (!Visited)
                Visited = true;
            Console.WriteLine($"You entered a {GetType().Name} tile");
            Console.WriteLine($"This is now your new respawn location.");
            player.UpdateRespawn(Location);
        }

        /// <summary>
        /// Unused
        /// </summary>
        /// <param name="player"></param>
        public void SetupParamsForTile(IPlayer player) { }
    }
}
