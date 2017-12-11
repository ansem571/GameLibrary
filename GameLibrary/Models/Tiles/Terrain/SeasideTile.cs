﻿using GameLibrary.Interfaces;
using System;

namespace GameLibrary.Models.Tiles.Terrain
{
    public class SeasideTile : ITile
    {
        public string RegionId { get; } = "Seaside";
        public IPoint Location { get; }
        public bool Visited { get; set; } = false;
        public SeasideTile(IPoint loc, string regionId, bool visited = false)
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
        }

        /// <summary>
        /// Unused
        /// </summary>
        /// <param name="player"></param>
        public void SetupParamsForTile(IPlayer player) { }
    }
}
