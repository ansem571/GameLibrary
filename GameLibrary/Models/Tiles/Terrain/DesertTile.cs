using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;

namespace GameLibrary.Models.Tiles.Terrain
{
    public class DesertTile : ITile
    {
        public string RegionId { get; } = "Desert";
        public IPoint Location { get; }
        public bool Visited { get; set; } = false;
        public DesertTile(IPoint loc, string regionId, bool visited = false)
        {
            Location = loc;
            RegionId = regionId;
            Visited = visited;
        }

        public void EnteredTile(IPlayer player, params object[] args)
        {
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
